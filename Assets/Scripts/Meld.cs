using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using System.Linq;
using System;

[Serializable]
public class Meld : IEquatable<Meld>
{
    //pythonライブラリインポート用の変数
    private dynamic mj_meld;
    public enum MeldType
    {
        CHI,
        PON,
        ANKAN,
        DAIMINKAN,
        SHOUMINKAN,
        NUKI
    }
    public MeldType meldType;
    public Tiles tiles;

    public Meld(MeldType type, Tiles tiles)
    {
        mj_meld = Py.Import("mahjong.meld");
        this.meldType = type;
        this.tiles = tiles;
    }
    public Meld(MeldType type, string man, string pin, string sou, string honor)
    {
        mj_meld = Py.Import("mahjong.meld");
        this.meldType = type;
        tiles = new Tiles(man, pin, sou, honor);
    }

    //鳴きの種類と牌の組み合わせが有効かどうかを返す
    public bool IsValid()
    {
        bool result;
        switch (meldType)
        {
            case MeldType.CHI:
                result = Is_CHI();
                break;
            case MeldType.PON:
                result = Is_PON();
                break;
            case MeldType.ANKAN:
            case MeldType.DAIMINKAN:
            case MeldType.SHOUMINKAN:
                result = Is_KAN();
                break;
            case MeldType.NUKI:
                result = true;
                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    /// <summary>
    /// チーが成立するかを判定する
    /// </summary>
    /// <returns></returns>
    private bool Is_CHI()
    {
        //条件→牌の総数が3つである・数牌である・連番である

        //牌の総数が3かどうかをチェックする
        if (tiles.TilesList.Count != 3) return false;

        //全て同じ種類の数牌かどうかをチェックする
        Tile.TileType tmpTileType = Tile.TileType.NONE;
        foreach (Tile t in tiles.TilesList)
        {
            //字牌が入っていたら不成立
            if (t.type == Tile.TileType.HONOR)
            {
                return false;
            }

            //牌の種類を記録し、異なる牌の種類があれば不成立;
            if (tmpTileType == Tile.TileType.NONE)
            {
                tmpTileType = t.type;
            }
            else
            {
                if (tmpTileType != t.type)
                {
                    return false;
                }
            }
        }

        //連番かどうかをチェックする
        //0(赤5を表す)を5に書き換える
        Tiles fixedTiles = tiles;
        if (fixedTiles.RemoveTileFromList(0, tmpTileType))
        {
            fixedTiles.AddTileToList("5", tmpTileType);
        }
        //ソートして比較する
        var _sortedTiles = fixedTiles.TilesList.OrderBy(t => t.Number).ToList();
        if ((_sortedTiles[0].Number == _sortedTiles[1].Number - 1) &&
            (_sortedTiles[0].Number == _sortedTiles[2].Number - 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// ポンが成立するかを判定する
    /// </summary>
    /// <returns></returns>
    private bool Is_PON()
    {
        //条件→牌の総数が3つである・数が同じである

        //牌の総数が3であるかをチェックする
        if (tiles.TilesList.Count != 3) return false;

        //数が同じであるかをチェックする
        if ((tiles.TilesList[0].Number == tiles.TilesList[1].Number) &&
            (tiles.TilesList[0].Number == tiles.TilesList[2].Number))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// カンが成立するかを判定する
    /// </summary>
    /// <returns></returns>
    private bool Is_KAN()
    {
        //条件→牌の総数が4つである・数が同じである

        //牌の総数が4であるかをチェックする
        if (tiles.TilesList.Count != 4) return false;

        //数が同じであるかをチェックする
        if ((tiles.TilesList[0].Number == tiles.TilesList[1].Number) &&
            (tiles.TilesList[0].Number == tiles.TilesList[2].Number) &&
            (tiles.TilesList[0].Number == tiles.TilesList[3].Number))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// pythonライブラリのmeldオブジェクトに変換して返す関数
    /// </summary>
    /// <returns></returns>
    public dynamic GetMeldObject()
    {
        MahjongUtils mahjongUtils = new MahjongUtils();
        //引数1 meld_type
        dynamic _meld_type;
        switch (meldType)
        {
            case MeldType.CHI:
                _meld_type = mj_meld.Meld.CHI;
                break;

            case MeldType.PON:
                _meld_type = mj_meld.Meld.PON;
                break;

            case MeldType.ANKAN:
            case MeldType.DAIMINKAN:
                _meld_type = mj_meld.Meld.KAN;
                break;

            case MeldType.SHOUMINKAN:
                _meld_type = mj_meld.Meld.SHOUMINKAN;
                break;
            default:
                _meld_type = mj_meld.Meld.NUKI;
                break;
        }

        //引数2   tiles136Array
        dynamic _tiles136Array = mahjongUtils.ConvertPredictionsTo136Array(tiles);

        //引数3   opend
        bool _opend = true;
        if (meldType == MeldType.ANKAN)
        {
            _opend = false;
        }


        return mj_meld.Meld(_meld_type, _tiles136Array, _opend);
    }

    bool IEquatable<Meld>.Equals(Meld other)
    {
        return meldType == other.meldType
            && tiles == other.tiles;
    }
}

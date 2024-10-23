using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using System.Linq;


public class MahjongUtils
{
    dynamic mj_tiles;
    dynamic mj_calculate;
    dynamic mj_shanten;
    public MahjongUtils()
    {
        mj_tiles = Py.Import("mahjong.tile");
        mj_calculate = Py.Import("mahjong.hand_calculating.hand");
        mj_shanten = Py.Import("mahjong.shanten");
    }


    /// <summary>
    /// 認識結果を136配列に変換する
    /// </summary>
    /// <param name="predictions"></param>
    /// <returns></returns>
    public dynamic ConvertPredictionsTo136Array(IPredictions predictions)
    {
        //萬子、筒子、索子、字牌の番号を割り当てる文字列
        string man = "", sou = "", pin = "", honor = "";

        List<Prediction> predictionsList = predictions.GetAllPredictions();
        foreach (Prediction p in predictionsList)
        {
            //1文字目は0~9の数字、2文字目は牌の種類
            //m:萬子 p:筒子 s:索子 z:字牌 b:裏面
            //数字の0は赤5として扱う。0pは赤5筒
            char num = p.class_name[0];
            char group = p.class_name[1];

            //牌の種類に対応したstring型変数に数字部分を追加する
            switch (group)
            {
                case 'm':
                    man += num.ToString();
                    break;
                case 'p':
                    pin += num.ToString();
                    break;
                case 's':
                    sou += num.ToString();
                    break;
                case 'z':
                    honor += num.ToString();
                    break;
                default:
                    break;
            }
        }

        //mahjongライブラリの関数で変換する
        var result = mj_tiles.TilesConverter.string_to_136_array(man, pin, sou, honor, true);
        return result;
    }

    public dynamic ConvertPredictionsTo136Array(Tiles tiles)
    {
        //萬子、筒子、索子、字牌の番号を割り当てる文字列
        string man = "", sou = "", pin = "", honor = "";

        foreach (Tile t in tiles.TilesList)
        {
            switch (t.type)
            {
                case Tile.TileType.MAN:
                    man += t.Number.ToString();
                    break;
                case Tile.TileType.PIN:
                    pin += t.Number.ToString();
                    break;
                case Tile.TileType.SOU:
                    sou += t.Number.ToString();
                    break;
                case Tile.TileType.HONOR:
                    honor += t.Number.ToString();
                    break;
                default:
                    break;
            }
        }

        var result = mj_tiles.TilesConverter.string_to_136_array(man, pin, sou, honor, true);
        return result;

    }

    /// <summary>
    /// 136配列を34配列に変換する
    /// </summary>
    /// <param name="tileArray_136"></param>
    /// <returns></returns>
    public dynamic To_34_array(dynamic tileArray_136)
    {
        var result = mj_tiles.TilesConverter.to_34_array(tileArray_136);
        return result;
    }

    /// <summary>
    /// 34配列からシャンテン数を計算する
    /// </summary>
    /// <param name="tileArray_34"></param>
    /// <returns></returns>
    public int GetShanten_from34Array(dynamic tileArray_34, bool use_chiitoitsu = true, bool use_kokushi = true)
    {
        int[] LengthCheckArray = tileArray_34;
        if (LengthCheckArray.Sum() > 14)
        {
            Debug.LogWarning("牌の数が多すぎます");
            return int.MaxValue;
        }
        var shantenCalculator = mj_shanten.Shanten();
        int shantenresult = shantenCalculator.calculate_shanten(tileArray_34, use_chiitoitsu, use_kokushi);
        return shantenresult;
    }

    /// <summary>
    /// 136配列からシャンテン数を計算する
    /// </summary>
    /// <param name="tileArray_136"></param>
    /// <returns></returns>
    public int GetShanten_from136Array(dynamic tileArray_136, bool use_chiitoitsu = true, bool use_kokushi = true)
    {
        //136配列を34配列に変換してシャンテン計算関数に投げる
        return GetShanten_from34Array(To_34_array(tileArray_136), use_chiitoitsu, use_kokushi);
    }

    /// <summary>
    /// Tilesクラスからシャンテン数を計算する
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    public int GetShanten(Tiles tiles)
    {
        bool _use_chiitoitsu = true;
        bool _use_kokushi = true;

        //副露がある場合
        if (tiles.MeldsList.Count > 0)
        {
            //七対子と国士無双は考慮から外す
            _use_chiitoitsu = false;
            _use_kokushi = false;

            foreach (Meld m in tiles.MeldsList)
            {
                if (m.IsValid() == false)
                {
                    Debug.LogWarning("不正な副露が含まれています");
                    return int.MaxValue;
                }
            }
        }

        //牌の数が14を超える場合は中断する
        //総数 = 牌の数 + 副露の数 * 3
        int tileTotalCount = tiles.TilesList.Count + tiles.MeldsList.Count * 3; //最後の*3は副露で晒される最小の牌の数　抜きドラは考慮していない
        if (tileTotalCount > 14)
        {
            Debug.LogWarning("牌の数が多すぎます");
            return int.MaxValue;
        }

        var array136 = ConvertPredictionsTo136Array(tiles);
        return GetShanten_from136Array(array136, _use_chiitoitsu, _use_kokushi);
    }

    /// <summary>
    /// 有効牌を返す関数
    /// </summary>
    /// <returns></returns>
    public List<Tile> GetEffectiveTiles(Tiles tiles)
    {
        //シャンテン数の評価が可能な枚数-1 を超えている場合は終了する
        if (tiles.TilesList.Count + tiles.MeldsList.Count * 3 > 13)
        {
            Debug.LogWarning("牌の数が多すぎます");
            return null;
        }

        List<Tile> result = new();
        int baseShanten = GetShanten(tiles);    //評価の基準となるシャンテン数
        Tiles tmpTiles = new Tiles(tiles);  //いろいろ弄るのでTilesの値をコピーしたリストを使う

        //37種類全ての牌を1枚追加したときのシャンテン数の変化を基に有効牌を取得する
        int maxTileID = 37;
        for (int i = 0; i < maxTileID; i++)
        {
            Tile t = new Tile(i);
            tmpTiles.AddTileToList(t);
            //シャンテン数が減っていた場合は返り値に追加する
            if (GetShanten(tmpTiles) < baseShanten)
            {
                result.Add(t);
            }
            tmpTiles.RemoveTileFromList(t);
        }
        return result;
    }

    /// <summary>
    /// 点数計算を行う関数
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="win_tile"></param>
    /// <param name="dora"></param>
    /// <param name="config"></param>
    /// <returns>HandResponseオブジェクト</returns>
    public HandResponse EstimateHandValue(Tiles tiles, Tile win_tile, List<Tile> dora, HandConfig config)
    {
        dynamic _calculator = mj_calculate.HandCalculator();
        HandResponse _handResponse = new HandResponse();

        //引数1   tiles: array with 14 tiles in 136-tile format
        dynamic _tile_136Array = ConvertPredictionsTo136Array(tiles);

        //引数2   win_tile: 136 format tile that caused win (ron or tsumo)
        Tiles win_tile_inTiles = new Tiles();
        win_tile_inTiles.AddTileToList(win_tile);
        dynamic _winTile_136Array = ConvertPredictionsTo136Array(win_tile_inTiles);

        //引数3   array with Meld objects
        List<dynamic> _meldObjects =new();
        foreach(Meld m in tiles.MeldsList)
        {
            _meldObjects.Add(m.GetMeldObject());
        }

        //引数4   array of tiles in 136-tile format
        Tiles dora_inTiles = new Tiles();
        foreach (Tile tile in dora)
        {
            dora_inTiles.AddTileToList(tile);
        }
        dynamic _dora_136Array = ConvertPredictionsTo136Array(dora_inTiles);

        //引数5   HandConfig object
        dynamic _configObject = config.GetHandConfig();

        //ここで計算
        dynamic result = _calculator.estimate_hand_value(_tile_136Array, _winTile_136Array, _meldObjects, _dora_136Array, _configObject);

        //dynamic resul から HandConigクラスへの変換
        _handResponse.cost_main = result.cost["main"];
        _handResponse.cost_aditional = result.cost["additionl"];
        _handResponse.han=result.han;
        _handResponse.fu=result.fu;
        _handResponse.yaku=result.yaku;

        return _handResponse;
    }

}

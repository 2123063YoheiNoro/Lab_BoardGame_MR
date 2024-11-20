using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class Tile : IEquatable<Tile>
{
    public Tile(int num, TileType tileType)
    {
        this.type = tileType;
        //numが0~9に収まるように修正する
        num = Mathf.Clamp(num, 0, 9);
        this.Number = num;
        this.Id_37 = (int)tileType * 10 + Number;
    }
    public Tile(int id_37)
    {
        this.Id_37 = id_37;
        //tileTypeは10の位
        this.type = (TileType)(id_37 / 10);
        //numberは1の位
        this.Number = id_37 % 10;
        //字牌は数牌と異なり1から始まるので1足しておく
        if (this.type == TileType.HONOR)
        {
            Number++;
        }
    }
    //コピー用コンストラクタ
    public Tile(Tile tile)
    {
        this.type = tile.type;
        this.Number = tile.Number;
        this.Id_37 = tile.Id_37;
    }

    public enum TileType
    {
        NONE = -1,
        MAN = 0,    //萬子
        PIN = 1,    //筒子
        SOU = 2,    //索子
        HONOR = 3   //字牌
    }

    [field: SerializeField] public TileType type { get; private set; } = TileType.NONE;
    /// <summary>
    /// 数牌 0:赤ドラ 1~9:対応する数
    /// 字牌 1:東 2:南 3:西 4:北 5:白 6:發 7:中
    /// </summary>
    [field: SerializeField] public int Number { get; private set; } = -1;
    [field: SerializeField] public int Id_37 { get; private set; }

    /// <summary>
    /// スプライトのアドレッサブルのアドレスを取得する関数
    /// </summary>
    /// <returns></returns>
    public string GetSpriteAddress()
    {
        //TileSprite_m1
        //ここハードコードなの気持ち悪い 余裕出来たら直す
        string pre = "TileSprite_";
        string group, number;
        number=this.Number.ToString();
        switch (this.type)
        {
            case TileType.MAN:
                group = "m";
                break;
            case TileType.PIN:
                group = "p";
                break;
            case TileType.SOU:
                group = "s";
                break;
            case TileType.HONOR:
                group = "z";
                break;
            default:    //break入ったらエラー確定だからここ必要？
                group = "";
                break;
        }
        return pre + group + number;
    }

    public bool Equals(Tile tile)
    {
        return this.Id_37 == tile.Id_37;
    }

    public override int GetHashCode()
    {
        return Id_37.GetHashCode();
    }
}

/// <summary>
/// 複数の牌をまとめて扱うためのクラス
/// </summary>
[Serializable]
public class Tiles : IEquatable<Tiles>
{
    public Tiles(string man = "", string pin = "", string sou = "", string honor = "", List<Meld> melds = null)
    {
        TilesList = new();
        AddTileToList(man, Tile.TileType.MAN);
        AddTileToList(pin, Tile.TileType.PIN);
        AddTileToList(sou, Tile.TileType.SOU);
        AddTileToList(honor, Tile.TileType.HONOR);
        if (melds != null)
        {
            this.MeldsList = new List<Meld>(melds);
        }
        else
        {
            MeldsList = new();
        }
    }
    //コピー用コンストラクタ
    public Tiles(Tiles tiles)
    {
        TilesList = new(tiles.TilesList);
        MeldsList = new(tiles.MeldsList);
    }

    public Tiles(IPredictions predictions)
    {
        TilesList = new();
        MeldsList = new();
        List<Prediction> predictionList = predictions.GetAllPredictions();
        //萬子、筒子、索子、字牌の番号を割り当てる文字列
        string man = "", sou = "", pin = "", honor = "";
        foreach (Prediction p in predictionList)
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

        AddTileToList(man, Tile.TileType.MAN);
        AddTileToList(pin, Tile.TileType.PIN);
        AddTileToList(sou, Tile.TileType.SOU);
        AddTileToList(honor, Tile.TileType.HONOR);
    }

    public Tiles()
    {
        TilesList = new();
        MeldsList = new();
    }

    /// <summary>
    /// 文字列から牌に変換してリストに追加する
    /// </summary>
    /// <param name="str"></param>
    /// <param name="type"></param>
    public void AddTileToList(string str, Tile.TileType type)
    {

        //文字列を1文字ずつ分割してタイルのインスタンスを生成する
        foreach (char c in str)
        {
            Tile t = new Tile((int)char.GetNumericValue(c), type);
            TilesList.Add(t);
        }
    }
    /// <summary>
    /// Tileクラスをリストに追加する
    /// </summary>
    /// <param name="t"></param>
    public void AddTileToList(Tile t)
    {
        //コピーを挟んで参照渡しを回避する
        Tile tile = new Tile(t);
        TilesList.Add(tile);
    }

    public bool RemoveTileFromList(int num, Tile.TileType type)
    {
        Tile t = new Tile(num, type);
        return TilesList.Remove(t);
    }
    public bool RemoveTileFromList(Tile tile)
    {
        return TilesList.Remove(tile);
    }

    /// <summary>
    /// 手牌を考慮して牌の残り枚数を返す関数
    /// </summary>
    /// <returns></returns>
    public int GetRemainingTilesCount(int num, Tile.TileType type)
    {
        Tile t = new Tile(num, type);
        int maxCountPerTile = 4;
        //赤5だったら牌の最大数は1
        if (num == 0)
        {
            maxCountPerTile = 1;
        }

        int result = maxCountPerTile - TilesList.Count(tile => tile == t);
        return result;
    }

    /// <summary>
    /// 現在の手牌で点数計算が可能かを返す関数
    /// </summary>
    /// <returns></returns>
    public bool IsValidHand()
    {
        //手牌と鳴き合わせて14枚ならあがれる
        //カンも3枚組として解釈すれば14固定で大丈夫
        int validTileCount = 14;
        int count = TilesList.Count + MeldsList.Count * 3;
        if (count == validTileCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Equals(Tiles other)
    {
        return TilesList.SequenceEqual(other.TilesList)
            && MeldsList.SequenceEqual(other.MeldsList);
    }

    [field: SerializeField] public List<Tile> TilesList { get; private set; }

    [field: SerializeField] public List<Meld> MeldsList { get; private set; }
}
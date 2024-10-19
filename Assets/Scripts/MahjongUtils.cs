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

        foreach (Tile t in tiles.tiles)
        {
            switch (t.type)
            {
                case Tile.TileType.MAN:
                    man += t.number.ToString();
                    break;
                case Tile.TileType.PIN:
                    pin += t.number.ToString();
                    break;
                case Tile.TileType.SOU:
                    sou += t.number.ToString();
                    break;
                case Tile.TileType.HONOR:
                    honor += t.number.ToString();
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
    public int GetShanten_from34Array(dynamic tileArray_34)
    {
        int[] LengthCheckArray = tileArray_34;
        if (LengthCheckArray.Sum() > 14)
        {
            Debug.LogWarning("牌の数が多すぎます");
            return int.MaxValue;
        }
        var shantenCalculator = mj_shanten.Shanten();
        int shantenresult = shantenCalculator.calculate_shanten(tileArray_34);
        return shantenresult;
    }

    /// <summary>
    /// 136配列からシャンテン数を計算する
    /// </summary>
    /// <param name="tileArray_136"></param>
    /// <returns></returns>
    public int GetShanten_from136Array(dynamic tileArray_136)
    {
        //136配列を34配列に変換してシャンテン計算関数に投げる
        return GetShanten_from34Array(To_34_array(tileArray_136));
    }
}

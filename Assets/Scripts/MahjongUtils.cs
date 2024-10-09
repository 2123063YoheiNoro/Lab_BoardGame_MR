using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
//using dynamic mj_tiles = Py.Import("mahjong.tile");


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
    public string ConvertPredictionsTo136Array(IPredictions predictions)
    {
        //萬子、筒子、索子、字牌の番号を割り当てる文字列
        string man = "", sou = "", pin = "", honor = "";
        string result = "";

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
                    man += group.ToString();
                    break;
                case 'p':
                    pin += group.ToString();
                    break;
                case 's':
                    sou += group.ToString();
                    break;
                case 'z':
                    honor += group.ToString();
                    break;
                default:
                    break;
            }

            //mahjongライブラリの関数で変換する
            result = mj_tiles.TilesConverter.string_to_136_array(man, pin, sou, honor);
        }

        return result;
    }
}

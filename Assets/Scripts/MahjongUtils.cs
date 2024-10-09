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
    /// �F�����ʂ�136�z��ɕϊ�����
    /// </summary>
    /// <param name="predictions"></param>
    /// <returns></returns>
    public string ConvertPredictionsTo136Array(IPredictions predictions)
    {
        //�ݎq�A���q�A���q�A���v�̔ԍ������蓖�Ă镶����
        string man = "", sou = "", pin = "", honor = "";
        string result = "";

        List<Prediction> predictionsList = predictions.GetAllPredictions();
        foreach (Prediction p in predictionsList)
        {
            //1�����ڂ�0~9�̐����A2�����ڂ͔v�̎��
            //m:�ݎq p:���q s:���q z:���v b:����
            //������0�͐�5�Ƃ��Ĉ����B0p�͐�5��
            char num = p.class_name[0];
            char group = p.class_name[1];

            //�v�̎�ނɑΉ�����string�^�ϐ��ɐ���������ǉ�����
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

            //mahjong���C�u�����̊֐��ŕϊ�����
            result = mj_tiles.TilesConverter.string_to_136_array(man, pin, sou, honor);
        }

        return result;
    }
}

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
    /// �F�����ʂ�136�z��ɕϊ�����
    /// </summary>
    /// <param name="predictions"></param>
    /// <returns></returns>
    public dynamic ConvertPredictionsTo136Array(IPredictions predictions)
    {
        //�ݎq�A���q�A���q�A���v�̔ԍ������蓖�Ă镶����
        string man = "", sou = "", pin = "", honor = "";

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

        //mahjong���C�u�����̊֐��ŕϊ�����
        var result = mj_tiles.TilesConverter.string_to_136_array(man, pin, sou, honor, true);
        return result;
    }

    public dynamic ConvertPredictionsTo136Array(Tiles tiles)
    {
        //�ݎq�A���q�A���q�A���v�̔ԍ������蓖�Ă镶����
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
    /// 136�z���34�z��ɕϊ�����
    /// </summary>
    /// <param name="tileArray_136"></param>
    /// <returns></returns>
    public dynamic To_34_array(dynamic tileArray_136)
    {
        var result = mj_tiles.TilesConverter.to_34_array(tileArray_136);
        return result;
    }

    /// <summary>
    /// 34�z�񂩂�V�����e�������v�Z����
    /// </summary>
    /// <param name="tileArray_34"></param>
    /// <returns></returns>
    public int GetShanten_from34Array(dynamic tileArray_34)
    {
        int[] LengthCheckArray = tileArray_34;
        if (LengthCheckArray.Sum() > 14)
        {
            Debug.LogWarning("�v�̐����������܂�");
            return int.MaxValue;
        }
        var shantenCalculator = mj_shanten.Shanten();
        int shantenresult = shantenCalculator.calculate_shanten(tileArray_34);
        return shantenresult;
    }

    /// <summary>
    /// 136�z�񂩂�V�����e�������v�Z����
    /// </summary>
    /// <param name="tileArray_136"></param>
    /// <returns></returns>
    public int GetShanten_from136Array(dynamic tileArray_136)
    {
        //136�z���34�z��ɕϊ����ăV�����e���v�Z�֐��ɓ�����
        return GetShanten_from34Array(To_34_array(tileArray_136));
    }
}

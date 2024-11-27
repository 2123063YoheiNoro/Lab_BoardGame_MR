//**************************************************
//�v�̔F�����玩���œ��_���v�Z����N���X
//**************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandScoreModel : MonoBehaviour
{
    private TilesReceiver tilesReceiver;
    private MahjongUtils mahjongUtils;
    public ReactiveProperty<HandResponse> rpHandResponse = new();
    private HandResponse handResponse;


    private void Start()
    {
        tilesReceiver = FindObjectOfType<TilesReceiver>();
        tilesReceiver.rpPredictions
            .Subscribe(p => OnPredictionReceived(p))
            .AddTo(this);

        mahjongUtils = new();
    }

    private void OnPredictionReceived(IPredictions predictions)
    {
        if (mahjongUtils == null)
        {
            mahjongUtils = new();
        }
        Tiles tiles = ConvertPredictionsToTiles(predictions);
        tiles.SortTiles();
        //�V�����e������-1�̂Ƃ��ɓ_���v�Z���s��
        //�a�����ɃV�����e������-1�ɂȂ�
        int shantenCount = mahjongUtils.GetShanten(tiles);
        if (shantenCount == -1)
        {
            rpHandResponse.Value = mahjongUtils.EstimateHandValue(tiles, tiles.TilesList[0], null, null);
            handResponse = mahjongUtils.EstimateHandValue(tiles, tiles.TilesList[0], null, null);
        }
    }
    private Tiles ConvertPredictionsToTiles(IPredictions predictions)
    {
        List<Prediction> predictionList = predictions.GetAllPredictions();

        //�ݎq�A���q�A���q�A���v�̔ԍ������蓖�Ă镶����
        string man = "", sou = "", pin = "", honor = "";
        foreach (Prediction p in predictionList)
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

        Tiles tiles = new Tiles(man, pin, sou, honor);
        return tiles;
    }
}

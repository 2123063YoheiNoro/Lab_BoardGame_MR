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
        //�V�����e������-1�̂Ƃ��A���v�̖�����14���̎��ɓ_���v�Z���s��.
        //python���̎d�l:�a�����ɃV�����e������-1�ɂȂ�.
        int tileCount = tiles.TilesList.Count + tiles.MeldsList.Count * 3;
        if (tileCount == 14)
        {
            int shantenCount = mahjongUtils.GetShanten(tiles);
            if (shantenCount == -1)
            {
                Tile winTile = GetWinTile(predictions);
                rpHandResponse.Value = mahjongUtils.EstimateHandValue(tiles, winTile, null, null);
                handResponse = mahjongUtils.EstimateHandValue(tiles, winTile, null, null);
            }
        }
        else
        {
            rpHandResponse.Value = new HandResponse();
        }
    }

    /// <summary>
    /// ������v���擾����֐�.
    /// </summary>
    /// <returns></returns>
    private Tile GetWinTile(IPredictions predictions)
    {
        //��ԉE�ɂ���v��������v�Ƃ���.
        List<Prediction> predictionList = predictions.GetAllPredictions();

        float maxX = 0;
        int index = 0;
        for (int i = 0; i < predictionList.Count; i++)
        {
            float x = predictionList[i].x;
            if (x > maxX)
            {
                maxX = x;
                index = i;
            }
        }

        char num = predictionList[index].class_name[0];
        char group = predictionList[index].class_name[1];
        int num_int = int.Parse(num.ToString());
        Tile.TileType tileType=Tile.TileType.NONE;
        switch (group)
        {
            case 'm':
                tileType = Tile.TileType.MAN;
                break;
            case 'p':
                tileType = Tile.TileType.PIN;
                break;
            case 's':
                tileType = Tile.TileType.SOU;
                break;
            case 'z':
                tileType = Tile.TileType.HONOR;
                break;
        }

        Tile winTile = new Tile(num_int, tileType);
        return winTile;
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

//**************************************************
//牌の認識から自動で得点を計算するクラス
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
        //シャンテン数が-1のとき、かつ牌の枚数が14枚の時に点数計算を行う.
        //python側の仕様:和了時にシャンテン数が-1になる.
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
    /// あがり牌を取得する関数.
    /// </summary>
    /// <returns></returns>
    private Tile GetWinTile(IPredictions predictions)
    {
        //一番右にある牌をあがり牌とする.
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

        Tiles tiles = new Tiles(man, pin, sou, honor);
        return tiles;
    }
}

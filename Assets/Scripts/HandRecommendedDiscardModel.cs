//***********************
//捨てると聴牌になる牌を計算するクラス.
//***********************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class HandRecommendedDiscardModel : MonoBehaviour
{
    private TilesReceiver _tilesReceiver;
    private MahjongUtils _mahjongUtils;

    private List<Tile> _latestRecTile = new List<Tile>();

    public Subject<List<Tile>> subject = new();


    void Start()
    {
        //牌の検出を受け取る.
        _tilesReceiver = FindObjectOfType<TilesReceiver>();
        _tilesReceiver.rpTiles
            .Subscribe(t => OnTileChanged(t))
            .AddTo(this);

        _mahjongUtils = new();
    }

    private void OnTileChanged(Tiles tiles)
    {
        //牌が14枚ある時、抜いた時に聴牌になる牌を取得する.

        if (_mahjongUtils == null) _mahjongUtils = new();
        List<Tile> recTileList = new List<Tile>();

        if (tiles.TilesList.Count + tiles.MeldsList.Count * 3 == 14)
        {
            //14枚の時点であがりが成立してたら計算しない.
            if (_mahjongUtils.GetShanten(tiles) != -1)
            {

                for (int i = 0; i < tiles.TilesList.Count; i++)
                {
                    //i番目を抜いてシャンテン数を計算する.
                    Tiles tmpTiles = new Tiles(tiles);
                    Tile removedTile = tmpTiles.TilesList[i];
                    tmpTiles.TilesList.RemoveAt(i);
                    int shanten = _mahjongUtils.GetShanten(tmpTiles);

                    //聴牌してたらリストに追加する.重複はしないようにする.
                    if (shanten == 0)
                    {
                        if (!recTileList.Contains(removedTile))
                        {
                            recTileList.Add(removedTile);
                        }
                    }
                }
            }
        }

        //牌のリストが変化していれば通知する.
        if (!recTileList.SequenceEqual(_latestRecTile))
        {
            subject.OnNext(recTileList);
        }
        _latestRecTile = new List<Tile>(recTileList);

    }
}

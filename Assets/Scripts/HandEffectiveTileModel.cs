using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandEffectiveTileModel : MonoBehaviour
{
    private TilesReceiver tilesReceiver;
    private MahjongUtils mahjongUtils;
    private List<Tile> effectiveTiles = new();
    private List<int> scores;    //点数の他に「役無し」のテキストが表示される場合がある
    public Subject<(List<Tile> effectiveTiles, List<int>scores)> subject = new();

    void Start()
    {
        tilesReceiver = FindObjectOfType<TilesReceiver>();
        tilesReceiver.rpTiles
            .Subscribe(t => OnTileChanged(t))
            .AddTo(this);

        mahjongUtils = new();
    }

    public void OnTileChanged(Tiles tiles)
    {
        effectiveTiles.Clear();
        scores.Clear();

        int shanten = mahjongUtils.GetShanten(tiles);
        //シャンテン数が0かつ牌の枚数が13枚のとき(つまり聴牌時)に有効牌を検索する
        if (shanten == 0
            && tiles.TilesList.Count + tiles.MeldsList.Count * 3 == 13)
        {
            //有効牌を取得して期待できる点数を計算する
            effectiveTiles = new List<Tile>(mahjongUtils.GetEffectiveTiles(tiles));
            //ここから点数の計算
            foreach (Tile t in effectiveTiles)
            {
                //聴牌状態の手牌に有効牌を加えて点数を計算する
                Tiles tiles_tmp = new Tiles(tiles);
                tiles_tmp.AddTileToList(t);
                //win_tileは有効牌
                HandResponse handResponse = mahjongUtils.EstimateHandValue(tiles_tmp, t);
                scores.Add(handResponse.cost_main);
            }
            //有効牌と点数が更新されたらSubjectを発行する
            subject.OnNext((effectiveTiles, scores));
        }
    }
}

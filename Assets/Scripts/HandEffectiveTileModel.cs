using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class HandEffectiveTileModel : MonoBehaviour
{
    private TilesReceiver _tilesReceiver;
    private MahjongUtils _mahjongUtils;
    private List<Tile> _effectiveTiles = new();
    private List<Tile> _latestEffectiveTile = new();    //変化を検出する用
    private List<int> _scores = new();    //演出の変化に使う予定だが未実装.
    private List<int> _latestScores = new();              //変化を検出する用
    public Subject<(List<Tile> effectiveTiles, List<int> scores)> subject = new();

    void Start()
    {
        //牌の検出を受け取る.
        _tilesReceiver = FindObjectOfType<TilesReceiver>();
        _tilesReceiver.rpTiles
            .Subscribe(t => OnTileChanged(t))
            .AddTo(this);

        _mahjongUtils = new();
    }

    public void OnTileChanged(Tiles tiles)
    {
        _effectiveTiles.Clear();
        _scores.Clear();
        if (_mahjongUtils == null) _mahjongUtils = new();

        int shanten = _mahjongUtils.GetShanten(tiles);
        Debug.Log("shanten  : " + shanten);
        //シャンテン数が0かつ牌の枚数が13枚のとき(つまり聴牌時)に有効牌を検索する
        if (shanten == 0
            && tiles.TilesList.Count + tiles.MeldsList.Count * 3 == 13)
        {
            //有効牌を取得して期待できる点数を計算する
            _effectiveTiles = new List<Tile>(_mahjongUtils.GetEffectiveTiles(tiles));
            //ここから点数の計算
            foreach (Tile t in _effectiveTiles)
            {
                //聴牌状態の手牌に有効牌を加えて点数を計算する
                Tiles tiles_tmp = new Tiles(tiles);
                tiles_tmp.AddTileToList(t);
                //win_tileは有効牌
                HandResponse handResponse = _mahjongUtils.EstimateHandValue(tiles_tmp, t);
                _scores.Add(handResponse.cost_main);
            }
            //有効牌か点数が更新されたらSubjectを発行する
            if (IsChangeResult())
            {
                subject.OnNext((_effectiveTiles, _scores));
            }
            _latestEffectiveTile=new List<Tile>(_effectiveTiles);
            _latestScores = new List<int>(_scores);

        }
        else
        {
            //有効牌か点数が更新されたらSubjectを発行する
            if (IsChangeResult())
            {
                subject.OnNext((_effectiveTiles, _scores));
            }
            _latestEffectiveTile = new List<Tile>(_effectiveTiles);
            _latestScores = new List<int>(_scores);
        }
    }

    /// <summary>
    /// 有効牌または点数が全開の計算から変動したかどうか.
    /// </summary>
    /// <returns></returns>
    private bool IsChangeResult()
    {
        //!A+!B=!(A*B) 変化あり+変化あり=!(変化なし*変化なし)
        //ANDの分だけちょっと軽くなるはず
        return !(_effectiveTiles.SequenceEqual(_latestEffectiveTile)
            && _scores.SequenceEqual(_latestScores));
    }
}

//**************************************************
//牌の認識から自動で シャンテン数を計算するクラス
//**************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandShantenModel : MonoBehaviour
{
    private TilesReceiver tilesReceiver;
    private MahjongUtils mahjongUtils;
    public ReactiveProperty<int> shanten ;

    void Start()
    {
        shanten = new();
        tilesReceiver = FindObjectOfType<TilesReceiver>();
        tilesReceiver.rpTiles
            .Subscribe(t => OnTileChanged(t))
            .AddTo(this);

        mahjongUtils = new();
    }

    /// <summary>
    /// シャンテン数計算するだけ
    /// </summary>
    /// <param name="tiles"></param>
    private void OnTileChanged(Tiles tiles)
    {
        if (mahjongUtils == null)
        {
            mahjongUtils = new();
        }
        shanten.Value = mahjongUtils.GetShanten(tiles);
    }


}

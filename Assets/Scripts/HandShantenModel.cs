//**************************************************
//�v�̔F�����玩���� �V�����e�������v�Z����N���X
//**************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandShantenModel : MonoBehaviour
{
    private TilesReceiver tilesReceiver;
    private MahjongUtils mahjongUtils;
    public ReactiveProperty<int> shanten =new() ;

    void Start()
    {
        tilesReceiver = FindObjectOfType<TilesReceiver>();
        tilesReceiver.rpTiles
            .Subscribe(t => OnTileChanged(t))
            .AddTo(this);

        mahjongUtils = new();
    }

    /// <summary>
    /// �V�����e�����v�Z���邾��
    /// </summary>
    /// <param name="tiles"></param>
    private void OnTileChanged(Tiles tiles)
    {
        if (mahjongUtils == null)
        {
            mahjongUtils = new();
        }

        if (tiles.TilesList.Count + tiles.MeldsList.Count * 3 >= 13)
        {
            shanten.Value = mahjongUtils.GetShanten(tiles);
        }
        else
        {
            shanten.Value = int.MinValue;
        }
        Debug.Log("OnTileChanged shanten : " + shanten);
    }


}

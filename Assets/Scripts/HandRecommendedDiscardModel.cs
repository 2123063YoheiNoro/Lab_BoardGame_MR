//***********************
//�̂Ă�ƒ��v�ɂȂ�v���v�Z����N���X.
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
        //�v�̌��o���󂯎��.
        _tilesReceiver = FindObjectOfType<TilesReceiver>();
        _tilesReceiver.rpTiles
            .Subscribe(t => OnTileChanged(t))
            .AddTo(this);

        _mahjongUtils = new();
    }

    private void OnTileChanged(Tiles tiles)
    {
        //�v��14�����鎞�A���������ɒ��v�ɂȂ�v���擾����.

        if (_mahjongUtils == null) _mahjongUtils = new();
        List<Tile> recTileList = new List<Tile>();

        if (tiles.TilesList.Count + tiles.MeldsList.Count * 3 == 14)
        {
            //14���̎��_�ł����肪�������Ă���v�Z���Ȃ�.
            if (_mahjongUtils.GetShanten(tiles) != -1)
            {

                for (int i = 0; i < tiles.TilesList.Count; i++)
                {
                    //i�Ԗڂ𔲂��ăV�����e�������v�Z����.
                    Tiles tmpTiles = new Tiles(tiles);
                    Tile removedTile = tmpTiles.TilesList[i];
                    tmpTiles.TilesList.RemoveAt(i);
                    int shanten = _mahjongUtils.GetShanten(tmpTiles);

                    //���v���Ă��烊�X�g�ɒǉ�����.�d���͂��Ȃ��悤�ɂ���.
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

        //�v�̃��X�g���ω����Ă���Βʒm����.
        if (!recTileList.SequenceEqual(_latestRecTile))
        {
            subject.OnNext(recTileList);
        }
        _latestRecTile = new List<Tile>(recTileList);

    }
}

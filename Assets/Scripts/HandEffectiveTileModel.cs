using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandEffectiveTileModel : MonoBehaviour
{
    private TilesReceiver tilesReceiver;
    private MahjongUtils mahjongUtils;
    public ReactiveCollection<Tile> effectiveTiles;

    void Start()
    {
        effectiveTiles = new ReactiveCollection<Tile>();
        tilesReceiver = FindObjectOfType<TilesReceiver>();
        tilesReceiver.rpTiles
            .Subscribe(t => OnTileChanged(t))
            .AddTo(this);

        mahjongUtils = new();
    }

    public void OnTileChanged(Tiles tiles)
    {
        int shanten = mahjongUtils.GetShanten(tiles);
        //�V�����e������0���v�̖�����13���̂Ƃ�(�܂蒮�v��)�ɗL���v����������
        if (shanten == 0
            && tiles.TilesList.Count + tiles.MeldsList.Count * 3 == 13)
        {
            List<Tile> tileTMP = mahjongUtils.GetEffectiveTiles(tiles);
            effectiveTiles = new ReactiveCollection<Tile>(tileTMP);
        }
        //���v����O�ꂽ��҂��v�Ȃ�
        else
        {
            effectiveTiles.Clear();
        }
    }
}

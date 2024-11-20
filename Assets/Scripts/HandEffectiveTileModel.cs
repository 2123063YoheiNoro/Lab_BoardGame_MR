using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandEffectiveTileModel : MonoBehaviour
{
    private TilesReceiver tilesReceiver;
    private MahjongUtils mahjongUtils;
    private List<Tile> effectiveTiles = new();
    private List<int> scores;    //�_���̑��Ɂu�𖳂��v�̃e�L�X�g���\�������ꍇ������
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
        //�V�����e������0���v�̖�����13���̂Ƃ�(�܂蒮�v��)�ɗL���v����������
        if (shanten == 0
            && tiles.TilesList.Count + tiles.MeldsList.Count * 3 == 13)
        {
            //�L���v���擾���Ċ��҂ł���_�����v�Z����
            effectiveTiles = new List<Tile>(mahjongUtils.GetEffectiveTiles(tiles));
            //��������_���̌v�Z
            foreach (Tile t in effectiveTiles)
            {
                //���v��Ԃ̎�v�ɗL���v�������ē_�����v�Z����
                Tiles tiles_tmp = new Tiles(tiles);
                tiles_tmp.AddTileToList(t);
                //win_tile�͗L���v
                HandResponse handResponse = mahjongUtils.EstimateHandValue(tiles_tmp, t);
                scores.Add(handResponse.cost_main);
            }
            //�L���v�Ɠ_�����X�V���ꂽ��Subject�𔭍s����
            subject.OnNext((effectiveTiles, scores));
        }
    }
}

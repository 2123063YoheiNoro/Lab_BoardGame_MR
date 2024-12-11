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
    private List<Tile> _latestEffectiveTile = new();    //�ω������o����p
    private List<int> _scores = new();    //���o�̕ω��Ɏg���\�肾��������.
    private List<int> _latestScores = new();              //�ω������o����p
    public Subject<(List<Tile> effectiveTiles, List<int> scores)> subject = new();

    void Start()
    {
        //�v�̌��o���󂯎��.
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
        //�V�����e������0���v�̖�����13���̂Ƃ�(�܂蒮�v��)�ɗL���v����������
        if (shanten == 0
            && tiles.TilesList.Count + tiles.MeldsList.Count * 3 == 13)
        {
            //�L���v���擾���Ċ��҂ł���_�����v�Z����
            _effectiveTiles = new List<Tile>(_mahjongUtils.GetEffectiveTiles(tiles));
            //��������_���̌v�Z
            foreach (Tile t in _effectiveTiles)
            {
                //���v��Ԃ̎�v�ɗL���v�������ē_�����v�Z����
                Tiles tiles_tmp = new Tiles(tiles);
                tiles_tmp.AddTileToList(t);
                //win_tile�͗L���v
                HandResponse handResponse = _mahjongUtils.EstimateHandValue(tiles_tmp, t);
                _scores.Add(handResponse.cost_main);
            }
            //�L���v���_�����X�V���ꂽ��Subject�𔭍s����
            if (IsChangeResult())
            {
                subject.OnNext((_effectiveTiles, _scores));
            }
            _latestEffectiveTile=new List<Tile>(_effectiveTiles);
            _latestScores = new List<int>(_scores);

        }
        else
        {
            //�L���v���_�����X�V���ꂽ��Subject�𔭍s����
            if (IsChangeResult())
            {
                subject.OnNext((_effectiveTiles, _scores));
            }
            _latestEffectiveTile = new List<Tile>(_effectiveTiles);
            _latestScores = new List<int>(_scores);
        }
    }

    /// <summary>
    /// �L���v�܂��͓_�����S�J�̌v�Z����ϓ��������ǂ���.
    /// </summary>
    /// <returns></returns>
    private bool IsChangeResult()
    {
        //!A+!B=!(A*B) �ω�����+�ω�����=!(�ω��Ȃ�*�ω��Ȃ�)
        //AND�̕�����������ƌy���Ȃ�͂�
        return !(_effectiveTiles.SequenceEqual(_latestEffectiveTile)
            && _scores.SequenceEqual(_latestScores));
    }
}

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
        //ƒVƒƒƒ“ƒeƒ“”‚ª0‚©‚Â”v‚Ì–‡”‚ª13–‡‚Ì‚Æ‚«(‚Â‚Ü‚è’®”v)‚É—LŒø”v‚ğŒŸõ‚·‚é
        if (shanten == 0
            && tiles.TilesList.Count + tiles.MeldsList.Count * 3 == 13)
        {
            List<Tile> tileTMP = mahjongUtils.GetEffectiveTiles(tiles);
            effectiveTiles = new ReactiveCollection<Tile>(tileTMP);
        }
        //’®”v‚©‚çŠO‚ê‚½‚ç‘Ò‚¿”v‚È‚µ
        else
        {
            effectiveTiles.Clear();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.UI;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.SocialPlatforms.Impl;

public class HandEffectiveTileView : MonoBehaviour
{
    [SerializeField] private GameObject _effectiveTilePrefab;
    private List<GameObject> _effectiveTileList = new List<GameObject>();

    public void OnEffectiveTileChanged((List<Tile>, List<int>) tuple)
    {
        _effectiveTileList.Clear();
        //有効牌と推測点数を結び付けるためのインデックス
        int index = 0;
        foreach (Tile tile in tuple.Item1)
        {
            //有効牌のアドレスを取得→スプライトを取得
            string spriteAddress = tile.GetSpriteAddress();
            Sprite sprite = Addressables.LoadAssetAsync<Sprite>(spriteAddress).WaitForCompletion();
            int score = tuple.Item2[index];

            GameObject effectiveTile = Instantiate(_effectiveTilePrefab);

            TMP_Text text = effectiveTile.GetComponentInChildren<TMP_Text>();
            Image image = effectiveTile.GetComponentInChildren<Image>();
            //期待得点が0点は役無し確定
            if (score <= 0)
            {
                //ハードコード気持ち悪いので暇なときに直す
                text.text = "役無し";
            }
            else
            {
                text.text = score.ToString();
            }
            if (sprite != null)
            {
                image.sprite = sprite;
            }

            _effectiveTileList.Add(effectiveTile);
            index++;
        }
    }

}

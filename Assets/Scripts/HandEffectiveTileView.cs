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
        //�L���v�Ɛ����_�������ѕt���邽�߂̃C���f�b�N�X
        int index = 0;
        foreach (Tile tile in tuple.Item1)
        {
            //�L���v�̃A�h���X���擾���X�v���C�g���擾
            string spriteAddress = tile.GetSpriteAddress();
            Sprite sprite = Addressables.LoadAssetAsync<Sprite>(spriteAddress).WaitForCompletion();
            int score = tuple.Item2[index];

            GameObject effectiveTile = Instantiate(_effectiveTilePrefab);

            TMP_Text text = effectiveTile.GetComponentInChildren<TMP_Text>();
            Image image = effectiveTile.GetComponentInChildren<Image>();
            //���ғ��_��0�_�͖𖳂��m��
            if (score <= 0)
            {
                //�n�[�h�R�[�h�C���������̂ŉɂȂƂ��ɒ���
                text.text = "�𖳂�";
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

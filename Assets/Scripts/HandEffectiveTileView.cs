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
    private List<GameObject> _effectiveTileList = new List<GameObject>();
    [SerializeField] private Transform parentTransform;
    [SerializeField] private float tileInterval = 0.028f;    //�����v�̉��̊Ԋu 0.28�͎�ł��v�̏c�̃T�C�Y(m).
    [SerializeField] private float instanceIntervalSec = 0.175f;    //�����v�𐶐�����Ԋu(�b).

    [SerializeField] private float rbDrag = 1;
    [SerializeField] private float rbAnglerDrag = 1;

    [SerializeField]private AudioSource _audioSource;

    private GameObject _eyeAnchor;  //�v�������^�[�Q�b�g.

    private Coroutine _latestCoroutine = null;

    public void OnEffectiveTileChanged((List<Tile>, List<int>) tuple)
    {
        Debug.Log("OnEffectiveTileChanged" + tuple.Item1.Count);
        //���҂ł���_���ɂ�鉉�o�̕ω��͗]�T������Ύ�������.
        //���͖�����.

        //�v�����̃R���[�`�����~����.
        if (_latestCoroutine != null)
        {
            StopCoroutine(_latestCoroutine);
        }
        //���ɃV�[����ɂ���v������.�L���v���Ȃ��Ȃ����Ƃ��������炷.
        if (_effectiveTileList.Count != 0)
        {
            ClearTiles(tuple.Item1.Count == 0);
        }
        //�v�̐������J�n����.
        if (tuple.Item1.Count != 0)
        {
            _latestCoroutine = StartCoroutine(InstanceTile(new List<Tile>(tuple.Item1)));
        }
    }
    /// <summary>
    /// �v�̃I�u�W�F�N�g���폜����
    /// </summary>
    private void ClearTiles(bool isPlaySE)
    {
        //�v��S���폜���ăA�h���b�T�u���̃����[�X�������s��.
        foreach (GameObject g in _effectiveTileList)
        {
            //Addressables.Release(g);//�����[�X������̂Ȃ����Č�����.
            GameObject.Destroy(g);
        }
        _effectiveTileList.Clear();

        if (isPlaySE)
        {
            //�v�����������̌��ʉ���炷
            AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
            AudioClip ac_disableTile = audioLibrary.GetAudioClip(AudioType.Shanten_DisableEffectiveTile);
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(ac_disableTile);
            }
            Addressables.Release(audioLibrary);
        }
    }

    private GameObject FindEyeAnchor()
    {
        return GameObject.Find("CenterEyeAnchor");
    }

    /// <summary>
    /// �L���v�̃I�u�W�F�N�g�𐶐�����.
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    IEnumerator InstanceTile(List<Tile> tiles)
    {
        yield return null;
        //�����̏���.
        AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        AudioClip ac_enableTile = audioLibrary.GetAudioClip(AudioType.Shanten_EnableEffectiveTile);
        AudioClip ac_tenpai = audioLibrary.GetAudioClip(AudioType.Shanten_Tenpai);

        //���v���̌��ʉ���炷.
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(ac_tenpai);
        }
        yield return new WaitForSeconds(0.5f);//0.5�b�͓K��.

        int index = 0;
        foreach (Tile tile in tiles)
        {
            //�L���v�̃A�h���X���擾.
            string tileAddress = tile.GetTilePrefabAddress();
            GameObject tilePrefab = Addressables.LoadAssetAsync<GameObject>(tileAddress).WaitForCompletion();
            //�����Ő���.
            GameObject tileObj = Instantiate(tilePrefab, parentTransform.position, tilePrefab.transform.rotation);
            //���X�g�ɒǉ����Đe�q�֌W��ݒ肷��.
            _effectiveTileList.Add(tileObj);
            tileObj.transform.SetParent(parentTransform);

            //�ʒu����.
            float posx = index;             //���Ԋu�ɔz�u����...
            posx -= (tiles.Count - 1) / 2.0f;  //�S�̂̒����̔��������炵�Ē����ɑ�����.
            posx *= tileInterval;           //���͂����Œ���.
            Vector3 targetLocalPos = new Vector3(posx, 0, 0);
            index++;

            //�R���|�[�l���g�ݒ�.
            //rigidbody����̐ݒ�.
            Rigidbody rb = tileObj.GetComponent<Rigidbody>();
            rb.drag = rbDrag;
            rb.angularDrag = rbAnglerDrag;
            //�����v�̐ݒ�
            FloatingTile ft = tileObj.AddComponent<FloatingTile>();
            ft.targetLocalPos = targetLocalPos;
            if (_eyeAnchor == null)
            {
                _eyeAnchor = FindEyeAnchor();
            }
            ft.lookingPos = _eyeAnchor.transform;

            //���ʉ���炷.
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(ac_enableTile);
            }

            yield return new WaitForSeconds(instanceIntervalSec);
        }
    }
}

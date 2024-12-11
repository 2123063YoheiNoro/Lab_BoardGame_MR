using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HandRecommendedDiscardView : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private float rbDrag = 1;
    [SerializeField] private float rbAnglerDrag = 1;
    [SerializeField] private float tileInterval=0.1f;
    [SerializeField] private AudioSource _audioSource;

    private GameObject _eyeAnchor;  //�v�������^�[�Q�b�g.

    private List<GameObject> recTilesList = new List<GameObject>();

    public void UpdateRecDiscard(List<Tile> recTiles)
    {
        //�V�[����ɔv������Ȃ����
        if (recTilesList.Count != 0)
        {
            //�V���������ߔv��0����������ʉ���炷.
            ClearRecTiles(recTiles.Count==0);
        }

        int index = 0;
        recTiles.Sort((a, b) => a.Id_37 - b.Id_37);
        foreach(Tile tile in recTiles)
        {
            //�L���v�̃A�h���X���擾.
            string tileAddress = tile.GetTilePrefabAddress();
            GameObject tilePrefab = Addressables.LoadAssetAsync<GameObject>(tileAddress).WaitForCompletion();
            //�����Ő���.
            GameObject tileObj = Instantiate(tilePrefab, parentTransform.position, tilePrefab.transform.rotation);
            //���X�g�ɒǉ����Đe�q�֌W��ݒ肷��.
            recTilesList.Add(tileObj);
            tileObj.transform.SetParent(parentTransform);

            //�ʒu����.
            float posx = index;             //���Ԋu�ɔz�u����...
            posx -= (recTiles.Count - 1) / 2.0f;  //�S�̂̒����̔��������炵�Ē����ɑ�����.
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
        }

        //�����̏���.
        AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        AudioClip ac_enableRecTile = audioLibrary.GetAudioClip(AudioType.RecommendedDIscard_Enable);


        //���ʉ���炷.
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(ac_enableRecTile);
        }
        Addressables.Release(audioLibrary);
    }

    //�V�[����̂������ߔv������.
    private void ClearRecTiles(bool playSE)
    {
        foreach(GameObject g in recTilesList)
        {
            GameObject.Destroy(g);
        }
        recTilesList.Clear();

        //���ʉ���炷.
        if (playSE)
        {
            AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
            AudioClip ac_disableRecTile = audioLibrary.GetAudioClip(AudioType.RecommendedDIscard_Disable);
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(ac_disableRecTile);
            }
            Addressables.Release(audioLibrary);
        }
    }


    private GameObject FindEyeAnchor()
    {
        return GameObject.Find("CenterEyeAnchor");
    }
}

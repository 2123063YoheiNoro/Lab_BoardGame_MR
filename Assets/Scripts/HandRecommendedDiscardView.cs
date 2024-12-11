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

    private GameObject _eyeAnchor;  //牌が向くターゲット.

    private List<GameObject> recTilesList = new List<GameObject>();

    public void UpdateRecDiscard(List<Tile> recTiles)
    {
        //シーン上に牌があるなら消す
        if (recTilesList.Count != 0)
        {
            //新しいお勧め牌が0個だったら効果音を鳴らす.
            ClearRecTiles(recTiles.Count==0);
        }

        int index = 0;
        recTiles.Sort((a, b) => a.Id_37 - b.Id_37);
        foreach(Tile tile in recTiles)
        {
            //有効牌のアドレスを取得.
            string tileAddress = tile.GetTilePrefabAddress();
            GameObject tilePrefab = Addressables.LoadAssetAsync<GameObject>(tileAddress).WaitForCompletion();
            //ここで生成.
            GameObject tileObj = Instantiate(tilePrefab, parentTransform.position, tilePrefab.transform.rotation);
            //リストに追加して親子関係を設定する.
            recTilesList.Add(tileObj);
            tileObj.transform.SetParent(parentTransform);

            //位置調整.
            float posx = index;             //等間隔に配置して...
            posx -= (recTiles.Count - 1) / 2.0f;  //全体の長さの半分をずらして中央に揃える.
            posx *= tileInterval;           //幅はここで調整.
            Vector3 targetLocalPos = new Vector3(posx, 0, 0);
            index++;


            //コンポーネント設定.
            //rigidbody周りの設定.
            Rigidbody rb = tileObj.GetComponent<Rigidbody>();
            rb.drag = rbDrag;
            rb.angularDrag = rbAnglerDrag;
            //浮く牌の設定
            FloatingTile ft = tileObj.AddComponent<FloatingTile>();
            ft.targetLocalPos = targetLocalPos;
            if (_eyeAnchor == null)
            {
                _eyeAnchor = FindEyeAnchor();
            }
            ft.lookingPos = _eyeAnchor.transform;
        }

        //音声の準備.
        AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        AudioClip ac_enableRecTile = audioLibrary.GetAudioClip(AudioType.RecommendedDIscard_Enable);


        //効果音を鳴らす.
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(ac_enableRecTile);
        }
        Addressables.Release(audioLibrary);
    }

    //シーン上のおすすめ牌を消す.
    private void ClearRecTiles(bool playSE)
    {
        foreach(GameObject g in recTilesList)
        {
            GameObject.Destroy(g);
        }
        recTilesList.Clear();

        //効果音を鳴らす.
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

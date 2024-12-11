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
    [SerializeField] private float tileInterval = 0.028f;    //麻雀牌の横の間隔 0.28は手打ち牌の縦のサイズ(m).
    [SerializeField] private float instanceIntervalSec = 0.175f;    //麻雀牌を生成する間隔(秒).

    [SerializeField] private float rbDrag = 1;
    [SerializeField] private float rbAnglerDrag = 1;

    [SerializeField]private AudioSource _audioSource;

    private GameObject _eyeAnchor;  //牌が向くターゲット.

    private Coroutine _latestCoroutine = null;

    public void OnEffectiveTileChanged((List<Tile>, List<int>) tuple)
    {
        Debug.Log("OnEffectiveTileChanged" + tuple.Item1.Count);
        //期待できる点数による演出の変化は余裕があれば実装する.
        //今は未実装.

        //牌生成のコルーチンを停止する.
        if (_latestCoroutine != null)
        {
            StopCoroutine(_latestCoroutine);
        }
        //既にシーン上にある牌を消す.有効牌がなくなったときだけ音鳴らす.
        if (_effectiveTileList.Count != 0)
        {
            ClearTiles(tuple.Item1.Count == 0);
        }
        //牌の生成を開始する.
        if (tuple.Item1.Count != 0)
        {
            _latestCoroutine = StartCoroutine(InstanceTile(new List<Tile>(tuple.Item1)));
        }
    }
    /// <summary>
    /// 牌のオブジェクトを削除する
    /// </summary>
    private void ClearTiles(bool isPlaySE)
    {
        //牌を全部削除してアドレッサブルのリリース処理を行う.
        foreach (GameObject g in _effectiveTileList)
        {
            //Addressables.Release(g);//リリースするものないって言われる.
            GameObject.Destroy(g);
        }
        _effectiveTileList.Clear();

        if (isPlaySE)
        {
            //牌が消えた時の効果音を鳴らす
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
    /// 有効牌のオブジェクトを生成する.
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    IEnumerator InstanceTile(List<Tile> tiles)
    {
        yield return null;
        //音声の準備.
        AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        AudioClip ac_enableTile = audioLibrary.GetAudioClip(AudioType.Shanten_EnableEffectiveTile);
        AudioClip ac_tenpai = audioLibrary.GetAudioClip(AudioType.Shanten_Tenpai);

        //聴牌時の効果音を鳴らす.
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(ac_tenpai);
        }
        yield return new WaitForSeconds(0.5f);//0.5秒は適当.

        int index = 0;
        foreach (Tile tile in tiles)
        {
            //有効牌のアドレスを取得.
            string tileAddress = tile.GetTilePrefabAddress();
            GameObject tilePrefab = Addressables.LoadAssetAsync<GameObject>(tileAddress).WaitForCompletion();
            //ここで生成.
            GameObject tileObj = Instantiate(tilePrefab, parentTransform.position, tilePrefab.transform.rotation);
            //リストに追加して親子関係を設定する.
            _effectiveTileList.Add(tileObj);
            tileObj.transform.SetParent(parentTransform);

            //位置調整.
            float posx = index;             //等間隔に配置して...
            posx -= (tiles.Count - 1) / 2.0f;  //全体の長さの半分をずらして中央に揃える.
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

            //効果音を鳴らす.
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(ac_enableTile);
            }

            yield return new WaitForSeconds(instanceIntervalSec);
        }
    }
}

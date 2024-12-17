using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.AddressableAssets;

public class HandScoreView_MR : MonoBehaviour
{
    [SerializeField] private Transform yakuTextParent;  //テキストの親,テキストが生成される場所.
    [SerializeField] private Vector3 _instancePosoffset = Vector3.zero;

    [SerializeField] private GameObject _preParticleEffect; //点数表示の前に生成されるパーティクル.
    [SerializeField] private GameObject _virtualHole;   //天井に空く穴.
    [SerializeField] private LayerMask _sceneMeshLayer; //天井のレイヤー.

    [SerializeField] private float TextSpeed = 0.5f;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float appearIntervalSec = 0.75f;    //文字の表示する間隔(秒).
    [SerializeField] private float displayTime = 5; //表示し続ける時間(秒).


    private Coroutine _latestCoroutine = null;  //コルーチン停止用.
    private List<GameObject> _yaku_textObjectList = new List<GameObject>();  //テキスト削除用.
    [SerializeField] private HandResponse _latestValidHandResponse = null;    //変化検知用.
    private bool _isCountDown = false;  //表示をやめるまでのカウントダウンフラグ.
    private bool _isInstanceCoroutineRunning = false;
    [SerializeField] private float _reminingTime = 0;

    //非表示までのカウントダウンをする.
    private void Update()
    {
        if (_isCountDown)
        {
            _reminingTime -= Time.deltaTime;
        }
        //カウントがなくなった、かつシーン上にテキストが残っている場合、かつコルーチンが走ってない場合.
        if (_reminingTime < 0 && _yaku_textObjectList.Count != 0 && !_isInstanceCoroutineRunning)
        {
            ClearScoreTextObject();
            _virtualHole.gameObject.SetActive(false);
            _latestValidHandResponse = null;
        }
    }

    public void UpdateResult(HandResponse handResponse)
    {
        Debug.Log("役更新 : " + handResponse.yaku);

        //有効なあがりでないなら何もしない.
        if (handResponse.cost_main == 0)
        {
            //非表示までのカウントダウン開始.
            if (!_isCountDown)
            {
                StartCountDown();
            }
            return;
        }
        //前回のあがりと同じなら何もしない.一瞬だけ牌の認識が途切れた時想定.
        else if (handResponse.Equals(_latestValidHandResponse))
        {
            StopCountDown();
            return;
        }
        else
        {
            StopCountDown();
        }

        _latestValidHandResponse = handResponse;

        //演出中に更新はいったら終了して新しく生成する.
        if (_latestCoroutine != null)
        {
            StopCoroutine(_latestCoroutine);
        }
        //シーン上の文字消して新しく生成.
        ClearScoreTextObject();
        _latestCoroutine = StartCoroutine(InstanceYakuTextObject(handResponse));
    }

    //シーン上の文字全部消す.
    public void ClearScoreTextObject()
    {
        //オブジェクト消して参照も消す.
        foreach (GameObject yaku in _yaku_textObjectList)
        {
            Destroy(yaku);
        }
        _yaku_textObjectList.Clear();
    }

    private IEnumerator InstanceYakuTextObject(HandResponse handResponse)
    {
        _isInstanceCoroutineRunning = true;

        //エフェクトの表示.
        Instantiate(_preParticleEffect, yakuTextParent.transform.position, _preParticleEffect.transform.rotation);
        _virtualHole.gameObject.SetActive(true);

        //テキストの生成位置(天井)を取得.
        Vector3 instancePos = GetInstantiatePosition();

        //音を鳴らす.
        AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        AudioClip ac_Agari_enable = audioLibrary.GetAudioClip(AudioType.Agari_Enable);
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(ac_Agari_enable);
        }

        yield return new WaitForSeconds(appearIntervalSec);

        //役をリスト化する.
        List<string> yaku_list = ConvertYaku_To_YakuList(handResponse.yaku);

        //役の文字を生成!.
        foreach (string yaku in yaku_list)
        {
            //役の文字オブジェクト生成.
            GameObject yakuTextPrefab = Addressables.LoadAssetAsync<GameObject>("Result_YakuText").WaitForCompletion();
            GameObject yakuText = Instantiate(yakuTextPrefab, instancePos, yakuTextParent.transform.rotation);

            //管理用のリストに追加.
            _yaku_textObjectList.Add(yakuText);
            //親設定.
            yakuText.transform.SetParent(yakuTextParent);


            //ここからコンポーネント関連.
            //テキストの設定.
            TMP_Text tmp_yaku = yakuText.GetComponent<TMP_Text>();
            tmp_yaku.text = yaku;
            tmp_yaku.ForceMeshUpdate();

            yield return null;

            //BoxColliderを追加する.
            BoxCollider boxCollider = yakuText.AddComponent<BoxCollider>();
            Bounds tmpBounds = tmp_yaku.bounds;
            boxCollider.size = new Vector3(tmpBounds.size.x, tmpBounds.size.y, 1);  //厚みを追加する.数値は適当.
            boxCollider.center = tmpBounds.center;

            //rigid body 下方向に力を加える.
            float yaku_speed = TextSpeed;
            Rigidbody yaku_rb = yakuText.GetComponent<Rigidbody>();
            yaku_rb.AddForce(-Vector3.up * yaku_speed, ForceMode.VelocityChange);

            //音を鳴らす
            AudioClip ac_enableYakuText = audioLibrary.GetAudioClip(AudioType.Agari_EnableYakuText);
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(ac_enableYakuText);
            }

            //次の文字生成までちょっと待つ.
            yield return new WaitForSeconds(appearIntervalSec);
        }

        //点数表示まで少し間を開ける.
        yield return new WaitForSeconds(appearIntervalSec);

        //点数の文字を生成する.
        InstanceScoreTextObject(handResponse);

        //音を鳴らす
        AudioClip ac_enableScoreText = audioLibrary.GetAudioClip(AudioType.Agari_EnableScoreText);
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(ac_enableScoreText);
        }


        Addressables.Release(audioLibrary);

        yield return new WaitForSeconds(appearIntervalSec);

        _isInstanceCoroutineRunning = false;
    }

    private void InstanceScoreTextObject(HandResponse handResponse)
    {
        //テキストの生成位置(天井)を取得.
        Vector3 instancePos = GetInstantiatePosition();

        GameObject scoreTextPrefab = Addressables.LoadAssetAsync<GameObject>("Result_ScoreText").WaitForCompletion();
        GameObject scoreText = Instantiate(scoreTextPrefab, instancePos, scoreTextPrefab.transform.rotation);

        //管理用のリストに追加.
        _yaku_textObjectList.Add(scoreText);
        //親設定.
        scoreText.transform.SetParent(yakuTextParent);

        //テキストの設定.
        TMP_Text tmp_score = scoreText.GetComponent<TMP_Text>();
        int cost = handResponse.cost_main + handResponse.cost_aditional * 2;
        tmp_score.text = cost.ToString();

        //rigid body 下方向に力を加える.
        float score_speed = 1.0f;
        Rigidbody score_rb = scoreText.GetComponent<Rigidbody>();
        score_rb.AddForce(-Vector3.up * score_speed, ForceMode.VelocityChange);
    }


    private Vector3 GetInstantiatePosition()
    {
        Vector3 result;
        //真上にレイを打って当たった場所を返す.
        Ray ray = new Ray(Vector3.up, yakuTextParent.position);
        RaycastHit hitInfo;
        float length = 5;
        //レイを飛ばす.
        if (Physics.Raycast(ray, out hitInfo, length, _sceneMeshLayer))
        {
            result = hitInfo.point + _instancePosoffset;
        }
        else
        {
            //レイが当たらなければ親の位置を返しておく.
            result = yakuTextParent.position;
        }
        return result;

    }

    private void StartCountDown()
    {
        _isCountDown = true;
        _reminingTime = displayTime;
    }

    private void StopCountDown()
    {
        _isCountDown = false;
        _reminingTime = displayTime;
    }

    //pythonライブラリからの入力をリストに変換する
    private List<string> ConvertYaku_To_YakuList(string yaku_string)
    {
        //[tannyao,dora,aka]のような１つの文字列からリストに変換する
        string trimmed = yaku_string.Trim('[', ']');
        List<string> result = new List<string>(trimmed.Split(','));

        return result;
    }
}

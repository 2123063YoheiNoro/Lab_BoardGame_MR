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

    private float appearIntervalSec = 0.75f;    //文字の表示する間隔(秒).
    private Coroutine _latestCoroutine = null;  //コルーチン停止用.
    private List<GameObject> _yaku_textObjectList = new List<GameObject>();  //テキスト削除用.

    public void UpdateResult(HandResponse handResponse)
    {
        Debug.Log("役更新");
        return;
        if (handResponse.cost_main == 0)
        {
            return;
        }

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

        //音を鳴らす.
        yield return new WaitForSeconds(appearIntervalSec);

        //役をリスト化する.
        List<string> yaku_list = ConvertYaku_To_YakuList(handResponse.yaku);

        //役の文字を生成!.
        foreach (string yaku in yaku_list)
        {
            //役の文字オブジェクト生成.
            InstanceYakuTextObject(yaku);

            //次の文字生成までちょっと待つ.
            yield return new WaitForSeconds(appearIntervalSec);
        }

        //点数表示まで少し間を開ける.
        yield return new WaitForSeconds(appearIntervalSec);

        //点数の文字を生成する.
        InstanceScoreTextObject(handResponse);
    }

    private void InstanceYakuTextObject(string yaku)
    {
        GameObject yakuTextPrefab = Addressables.LoadAssetAsync<GameObject>("Result_YakuText").WaitForCompletion();
        GameObject yakuText = Instantiate(yakuTextPrefab, yakuTextParent.position, yakuTextPrefab.transform.rotation);
        //管理用のリストに追加.
        _yaku_textObjectList.Add(yakuText);
        //親設定.
        yakuText.transform.SetParent(yakuTextParent);
        //ここからコンポーネント関連.
        //テキストの設定.
        TMP_Text tmp_yaku = yakuText.GetComponent<TMP_Text>();
        tmp_yaku.text = yaku;
        //BoxColliderを追加する.テキストの文字を変更してから追加すると勝手に大きさを調整してくれるのでここで追加する.
        BoxCollider boxCollider = yakuText.AddComponent<BoxCollider>();
        //厚みが0なので設定する.とりあえず5cm.
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0.05f);
        //rigid body 上方向に力を加える.
        float yaku_speed = 1.0f;
        Rigidbody yaku_rb = yakuText.GetComponent<Rigidbody>();
        yaku_rb.AddForce(Vector3.up * yaku_speed, ForceMode.VelocityChange);
    }

    private void InstanceScoreTextObject(HandResponse handResponse)
    {
        GameObject scoreTextPrefab = Addressables.LoadAssetAsync<GameObject>("Result_ScoreText").WaitForCompletion();
        GameObject scoreText = Instantiate(scoreTextPrefab, yakuTextParent.position, scoreTextPrefab.transform.rotation);
        //管理用のリストに追加.
        _yaku_textObjectList.Add(scoreText);
        //親設定.
        scoreText.transform.SetParent(yakuTextParent);
        //テキストの設定.
        TMP_Text tmp_score = scoreText.GetComponent<TMP_Text>();
        int cost = handResponse.cost_main + handResponse.cost_aditional * 2;
        tmp_score.text = cost.ToString();
        //rigid body 上方向に力を加える.
        float score_speed = 1.0f;
        Rigidbody score_rb = scoreText.GetComponent<Rigidbody>();
        score_rb.AddForce(Vector3.up * score_speed, ForceMode.VelocityChange);
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

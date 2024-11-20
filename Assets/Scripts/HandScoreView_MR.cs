using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HandScoreView_MR : MonoBehaviour
{
    [SerializeField] GameObject yaku_prefub;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject yaku_parent;
    [SerializeField] TMP_Text cost; //点数
    [SerializeField] TMP_Text cost_detail;//支払いの詳細を表示するテキスト 2000オールとか18000,6000とか
    [SerializeField] TMP_Text han;  //はん　単位もここで入れる
    [SerializeField] TMP_Text fu;   //符　これも単位込み

    private float appearIntervalSec = 0.75f;
    private List<GameObject> yaku_objectList = new();

    private IEnumerator latestC;

    public void UpdateResult(HandResponse handResponse)
    {
        Debug.Log("役更新");
        StopAllCoroutines();
        StartCoroutine(InstanceYakuUI(handResponse));
    }

    //画面上の役全部消す UIも消す
    public void ClearScore()
    {
        foreach (GameObject yaku in yaku_objectList)
        {
            Destroy(yaku);
        }
        canvas.SetActive(false);
        cost.text = "";
        cost_detail.text = "";
        han.text = "";
        fu.text = "";
    }

    IEnumerator InstanceYakuUI(HandResponse handResponse)
    {
        //ClearScore();
        canvas.SetActive(true);
        List<string> yaku_list = ConvertYaku_To_YakuList(handResponse.yaku);
        //役の名前とはん数を適用したテキストを作成する
        foreach (string yaku in yaku_list)
        {
            GameObject yaku_element = Instantiate(yaku_prefub);
            yaku_objectList.Add(yaku_element);
            yaku_element.transform.SetParent(yaku_parent.transform, false);
            yaku_element.SetActive(false);

            //プレハブの子が役名テキスト、孫がはんテキスト
            Transform yaku_name = yaku_element.transform.GetChild(0);
            Transform han = yaku_name.transform.GetChild(0);
            TMP_Text yaku_name_text = yaku_name.GetComponent<TMP_Text>();
            TMP_Text han_text = han.GetComponent<TMP_Text>();

            yaku_name_text.text = yaku;
            han_text.text = ""; //役ごとのはんの取得は複雑なので後回し
        }

        //役を一つずつ表示する
        foreach (GameObject yaku in yaku_objectList)
        {
            //有効化して次の役の表示まで待つ
            yaku.SetActive(true);
            yield return new WaitForSeconds(appearIntervalSec);
        }

        //点数を表示する
        int _cost = handResponse.cost_main + handResponse.cost_aditional * 2;
        cost.text = _cost.ToString();
        cost_detail.text = String.Format("{0} - {1}", handResponse.cost_main.ToString(), handResponse.cost_aditional.ToString());
        han.text = handResponse.han.ToString();
        fu.text= handResponse.fu.ToString();
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

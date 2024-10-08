using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PredictionVisualiserModel : MonoBehaviour
{
    private IPredictions _predictions;
    private TilesReceiver tilesReceiver;
    //要素のリスト
    [SerializeField] private List<PredictionVisualiserElement> _elements;
    //要素のプレハブ
    [SerializeField] private GameObject elementPrefab;
    [SerializeField] private GameObject elementParent;

    [SerializeField] float size = 1;

    private void Start()
    {
        _elements = new();
        tilesReceiver = FindObjectOfType<TilesReceiver>();
    }

    private void Update()
    {
        //認識結果を取得
        List<Prediction> predictionsList = tilesReceiver.predictions.GetAllPredictions();

        //認識結果のサイズが要素リストのサイズより大きい場合
        if (predictionsList.Count > _elements.Count)
        {
            for (int i = 0; i < predictionsList.Count - _elements.Count; i++)
            {
                //要素を生成してリストに追加する
                GameObject elementObj = Instantiate(elementPrefab);
                elementObj.transform.SetParent(elementParent.transform);
                PredictionVisualiserElement element = elementObj.GetComponent<PredictionVisualiserElement>();
                _elements.Add(element);
                //このまま実行すると値の設定が完了していない状態で画面に写るので非表示にして終了する
                elementObj.SetActive(false);
            }

            return;
        }

        //要素のリスト分走査する
        int num = 0;
        foreach (PredictionVisualiserElement e in _elements)
        {
            //要素数が認識結果のサイズより大きい場合は非表示にする
            if (predictionsList.Count > num)
            {
                //要素を有効化して値を設定する
                e.gameObject.SetActive(true);
                Prediction p = predictionsList[num];
                e.SetPosition(p.x, p.y, size);
                e.SetRange(p.width * size, p.height * size);
                e.SetLabelText(p.class_name);
                e.SetRandomColor(p.class_id);
            }
            else
            {
                //要素を非表示にする
                e.gameObject.SetActive(false);
            }
            num++;
        }
    }
}

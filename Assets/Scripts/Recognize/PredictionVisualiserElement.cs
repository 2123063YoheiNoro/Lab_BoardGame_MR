//認識結果表示の要素.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PredictionVisualiserElement : MonoBehaviour
{
    private TMP_Text labelText;
    private Image rangeImage;
    private RectTransform rectTransform;
    //認識に使う画像のサイズ(規定)
    private float imageX = 640;
    private float imageY = 480;

    void Start()
    {
        labelText = GetComponentInChildren<TMP_Text>();
        rangeImage = GetComponentInChildren<Image>();
        rectTransform = GetComponent<RectTransform>();
    }


    //xとyの処理
    public void SetPosition(float x, float y, float scale)
    {
        //画面中央の座標が0,0になるように合わせる
        float fixedX = (x - imageX / 2) * scale;
        float fixedY = -(y - imageY / 2) * scale;
        rectTransform.anchoredPosition = new Vector3(fixedX, fixedY, 0);
    }
    //widthとheightの処理
    public void SetRange(float width, float height)
    {
        rectTransform.sizeDelta = new Vector2(width, height);
    }
    //classの処理
    public void SetLabelText(string t)
    {
        labelText.text = t;
    }
    public void SetScale(float x, float y)
    {
        rectTransform.localScale = new Vector3(x, y, 0);
    }
    //色の処理 predictionsとは関係ない
    public void SetRandomColor(int seed)
    {
        Random.InitState(seed);
        //黒に近い色を避けながらランダムで色を決める
        float h, s, v;
        h = Random.Range(0.0f, 1.0f);
        s = Random.Range(0.5f, 1.0f);
        v = 1;

        Color col = Color.HSVToRGB(h, s, v);
        rangeImage.color = col;
    }
}

//�F�����ʕ\���̗v�f.
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
    //�F���Ɏg���摜�̃T�C�Y(�K��)
    private float imageX = 640;
    private float imageY = 480;

    void Start()
    {
        labelText = GetComponentInChildren<TMP_Text>();
        rangeImage = GetComponentInChildren<Image>();
        rectTransform = GetComponent<RectTransform>();
    }


    //x��y�̏���
    public void SetPosition(float x, float y, float scale)
    {
        //��ʒ����̍��W��0,0�ɂȂ�悤�ɍ��킹��
        float fixedX = (x - imageX / 2) * scale;
        float fixedY = -(y - imageY / 2) * scale;
        rectTransform.anchoredPosition = new Vector3(fixedX, fixedY, 0);
    }
    //width��height�̏���
    public void SetRange(float width, float height)
    {
        rectTransform.sizeDelta = new Vector2(width, height);
    }
    //class�̏���
    public void SetLabelText(string t)
    {
        labelText.text = t;
    }
    public void SetScale(float x, float y)
    {
        rectTransform.localScale = new Vector3(x, y, 0);
    }
    //�F�̏��� predictions�Ƃ͊֌W�Ȃ�
    public void SetRandomColor(int seed)
    {
        Random.InitState(seed);
        //���ɋ߂��F������Ȃ��烉���_���ŐF�����߂�
        float h, s, v;
        h = Random.Range(0.0f, 1.0f);
        s = Random.Range(0.5f, 1.0f);
        v = 1;

        Color col = Color.HSVToRGB(h, s, v);
        rangeImage.color = col;
    }
}

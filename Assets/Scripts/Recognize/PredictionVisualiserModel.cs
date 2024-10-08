using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PredictionVisualiserModel : MonoBehaviour
{
    private IPredictions _predictions;
    private TilesReceiver tilesReceiver;
    //�v�f�̃��X�g
    [SerializeField] private List<PredictionVisualiserElement> _elements;
    //�v�f�̃v���n�u
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
        //�F�����ʂ��擾
        List<Prediction> predictionsList = tilesReceiver.predictions.GetAllPredictions();

        //�F�����ʂ̃T�C�Y���v�f���X�g�̃T�C�Y���傫���ꍇ
        if (predictionsList.Count > _elements.Count)
        {
            for (int i = 0; i < predictionsList.Count - _elements.Count; i++)
            {
                //�v�f�𐶐����ă��X�g�ɒǉ�����
                GameObject elementObj = Instantiate(elementPrefab);
                elementObj.transform.SetParent(elementParent.transform);
                PredictionVisualiserElement element = elementObj.GetComponent<PredictionVisualiserElement>();
                _elements.Add(element);
                //���̂܂܎��s����ƒl�̐ݒ肪�������Ă��Ȃ���Ԃŉ�ʂɎʂ�̂Ŕ�\���ɂ��ďI������
                elementObj.SetActive(false);
            }

            return;
        }

        //�v�f�̃��X�g����������
        int num = 0;
        foreach (PredictionVisualiserElement e in _elements)
        {
            //�v�f�����F�����ʂ̃T�C�Y���傫���ꍇ�͔�\���ɂ���
            if (predictionsList.Count > num)
            {
                //�v�f��L�������Ēl��ݒ肷��
                e.gameObject.SetActive(true);
                Prediction p = predictionsList[num];
                e.SetPosition(p.x, p.y, size);
                e.SetRange(p.width * size, p.height * size);
                e.SetLabelText(p.class_name);
                e.SetRandomColor(p.class_id);
            }
            else
            {
                //�v�f���\���ɂ���
                e.gameObject.SetActive(false);
            }
            num++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandScorePresenter : MonoBehaviour
{
    [SerializeField] private HandScoreModel model;
    [SerializeField] private HandScoreView view;
    void Start()
    {
        //���f���̓_���v�Z���ʂ��r���[�ɓo�^����
        model.rpHandResponse
            .Subscribe(hr => view.UpdateText(hr))
            .AddTo(this);
    }
}

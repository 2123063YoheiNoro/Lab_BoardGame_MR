using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandScorePresenter : MonoBehaviour
{
    private HandScoreModel _model;
    [SerializeField] private HandScoreView view;

    // Start is called before the first frame update
    private void Awake()
    {
        _model = gameObject.AddComponent<HandScoreModel>();

    }
    void Start()
    {
        //���f���̓_���v�Z���ʂ��r���[�ɓo�^����
        _model.rpHandResponse
            .Subscribe(hr => view.UpdateText(hr))
            .AddTo(this);
    }
}

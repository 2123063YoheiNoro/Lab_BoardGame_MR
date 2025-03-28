using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandScorePresenter : MonoBehaviour
{
    [SerializeField] private HandScoreModel model;
    [SerializeField] private HandScoreView view;
    [SerializeField] private HandScoreView_MR view_MR;
    void Start()
    {
        //モデルの点数計算結果をビューに登録する
        if (view != null)
        {
            model.rpHandResponse
                .Subscribe(hr => view.UpdateText(hr))
                .AddTo(this);
        }

        if (view_MR != null)
        {
            model.rpHandResponse
                .Subscribe(hr => view_MR.UpdateResult(hr))
                .AddTo(this);
        }
    }
}

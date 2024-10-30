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
        //ƒ‚ƒfƒ‹‚Ì“_”ŒvŽZŒ‹‰Ê‚ðƒrƒ…[‚É“o˜^‚·‚é
        model.rpHandResponse
            .Subscribe(hr => view.UpdateText(hr))
            .AddTo(this);
    }
}

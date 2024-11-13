using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandEffectiveTilePresenter : MonoBehaviour
{
    [SerializeField] private HandEffectiveTileModel model;
    [SerializeField] private HandEffectiveTileView view;

    private void Start()
    {
        model.subject
            .Subscribe(view.OnEffectiveTileChanged)
            .AddTo(this);
    }
}

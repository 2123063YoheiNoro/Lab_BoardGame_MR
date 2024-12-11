using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandRecommendedDiscardPresenter : MonoBehaviour
{
    [SerializeField]private HandRecommendedDiscardModel model;
    [SerializeField] private HandRecommendedDiscardView view;

    private void Start()
    {
        model.subject
            .Subscribe(view.UpdateRecDiscard)
            .AddTo(this);
    }

}

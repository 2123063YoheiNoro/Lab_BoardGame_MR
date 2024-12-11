using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandShantenPresenter : MonoBehaviour
{
    [SerializeField]private HandShantenView shantenView;
    [SerializeField]private HandShantenModel shantenModel;
    private void Start()
    {
        //モデルのシャンテン数をビューに登録する
        shantenModel.shanten
            .Subscribe(shantenView.UpdateText)
            .AddTo(this);
    }
}

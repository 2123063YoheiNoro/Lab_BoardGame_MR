using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorModeDisplayController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _editorModeObjects;    // 編集モードの時だけ表示されるオブジェクト
    private bool _isEditorMode = false; // 編集モードかどうかのフラグ

    //ハンドトラッキング時の検出用.
    [SerializeField] private OVRHand _OVRHand;
    private bool _wasGuestureDetected;  //前フレームとの比較用.

    private void Start()
    {
        //初期化.
        ToggleEditorModeObjects(_isEditorMode);
    }

    private void Update()
    {
        if (_OVRHand.IsTracked)
        {
            //ジェスチャーを認識したタイミングでモードを切り替え.
            if ((_wasGuestureDetected != IsGestureDetected()) && IsGestureDetected())
            {
                Debug.Log("toggled");
                ToggleEditorMode();
            }
            _wasGuestureDetected = IsGestureDetected();
        }
    }

    private bool IsGestureDetected()
    {
        Debug.Log($"{_OVRHand.GetFingerIsPinching(OVRHand.HandFinger.Middle)},{_OVRHand.GetFingerIsPinching(OVRHand.HandFinger.Ring)}");
        return _OVRHand.GetFingerIsPinching(OVRHand.HandFinger.Ring);
    }


    // 編集モードに応じてオブジェクトを表示/非表示にする
    private void ToggleEditorModeObjects(bool isEditorMode)
    {
        foreach (var obj in _editorModeObjects)
        {
            if (obj != null)
            {
                obj.SetActive(isEditorMode); // isEditorModeがtrueなら表示、falseなら非表示
            }
        }
    }

    // 編集モードを切り替えるメソッド
    public void ToggleEditorMode()
    {
        _isEditorMode = !_isEditorMode;
        ToggleEditorModeObjects(_isEditorMode); // 切り替えた後に表示/非表示を更新
    }
}

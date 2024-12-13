//******************************
//オブジェクトを別のオブジェクトに追従させるクラス
//******************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PositionFollower : MonoBehaviour
{
    // 追従対象のオブジェクト
    public Transform target;

    // 初期オフセット値（追従対象との初期相対距離）
    private Vector3 initialOffset;

    private void Start()
    {
        if (target != null)
        {
            // 初期オフセットを計算
            initialOffset = transform.position - target.position;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // 初期オフセットを保ちながら追従
            transform.position = target.position + initialOffset;
        }
    }

    private void OnValidate()
    {
        if (target != null)
        {
            // 初期オフセットを再計算（ターゲット変更時などに対応）
            initialOffset = transform.position - target.position;
        }
    }
}

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

    [SerializeField] private bool followRotationX = false; // X軸の回転を追従するか
    [SerializeField] private bool followRotationY = false; // Y軸の回転を追従するか
    [SerializeField] private bool followRotationZ = false; // Z軸の回転を追従するか

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


        // 回転の追従
        if (followRotationX || followRotationY || followRotationZ)
        {
            Vector3 targetEulerAngles = target.rotation.eulerAngles;
            Vector3 currentEulerAngles = transform.rotation.eulerAngles;

            transform.rotation = Quaternion.Euler(
                followRotationX ? targetEulerAngles.x : currentEulerAngles.x,
                followRotationY ? targetEulerAngles.y : currentEulerAngles.y,
                followRotationZ ? targetEulerAngles.z : currentEulerAngles.z
            );
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

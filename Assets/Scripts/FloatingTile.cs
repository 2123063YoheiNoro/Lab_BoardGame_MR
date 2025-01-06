//****************************
//rigid bodyの牌を空中に固定する
//****************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTile : MonoBehaviour
{
    [SerializeField] public Vector3 targetLocalPos;
    [SerializeField] public Transform lookingPos;
    [SerializeField] public float speed = 1;
    [SerializeField] public float torqueStrength = 0.01f;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //位置の処理.
        //ターゲットのローカル座標をワールド座標に変換して差分を計算する.
        Vector3 targetPos = transform.parent.TransformPoint(targetLocalPos);
        Vector3 dir = targetPos - transform.position;
        _rb.AddForce(dir * speed, ForceMode.Force);

        //回転の処理.
        if (lookingPos == null) return;
        Vector3 targetRotPos = lookingPos.transform.position;
        Vector3 rotDir = targetRotPos - transform.position;
        //オブジェクトの正面側がターゲットに向くようにする.
        Quaternion fixedRotDir = Quaternion.FromToRotation(transform.forward, rotDir) * transform.rotation;
        //ここから下はchatgpt.
        Quaternion deltaRotation = fixedRotDir * Quaternion.Inverse(transform.rotation);
        Vector3 axis;
        float angle;
        deltaRotation.ToAngleAxis(out angle, out axis);

        // 小さな角度の回転は無視するため、閾値を設定.
        if (angle > 0.1f)
        {
            // 回転軸に沿ってトルクを加える.
            _rb.AddTorque(axis * angle * torqueStrength, ForceMode.VelocityChange);
        }
    }
}

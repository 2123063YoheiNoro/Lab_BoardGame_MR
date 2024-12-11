//************************
//GameObjectをある方向に向き続けさせるクラス
//************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    //ほぼchatgpt産.すごいね.
    [SerializeField] private Transform target;

    private void Update()
    {
        if (target != null)
        {
            Vector3 dir = target.position - this.gameObject.transform.position;
            //向きが反対だったのでここだけ修正.
            RotateTowardsOnYAxis(-dir);
        }
    }
    private void RotateTowardsOnYAxis(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            direction.y = 0;
            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Slerpを使用してスムーズにY軸回転
            Quaternion smoothedRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, smoothedRotation, Time.deltaTime * 5f);
        }
    }
}

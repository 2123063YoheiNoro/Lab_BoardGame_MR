using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RiichiStickReceiver : MonoBehaviour
{
    public Subject<Unit> subject = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RiichiStick"))
        {
            other.gameObject.SetActive(false);
            subject.OnNext(Unit.Default);
        }
    }
}

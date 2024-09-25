using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TilesReceiver))]
public class MahjongUtils : MonoBehaviour
{
    TilesReceiver tilesReceiver;
    Predictions predictions;
    void Start()
    {
        tilesReceiver = GetComponent<TilesReceiver>();
    }

    // Update is called once per frame
    void Update()
    {
        predictions = tilesReceiver.predictions;

    }

    /*
    (string man,string pin,string sou,string honors) ConvertToPythonMahjong()
    {

    }
    */
}

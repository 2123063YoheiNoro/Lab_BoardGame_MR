using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Predictions
{
    public Prediction[] predictions;
}

[System.Serializable]
public class Prediction
{
    public float x;
    public float y;
    public float width;
    public float height;
    public float confidence;
    public string class_name;
    public int class_id;
}

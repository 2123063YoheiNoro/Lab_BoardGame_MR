using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPredictions
{
    List<Prediction> GetAllPredictions();
}

[System.Serializable]
public class Predictions:IPredictions
{
    public Prediction[] predictions;

    public List<Prediction> GetAllPredictions()
    {
        //”z—ñ‚ðƒŠƒXƒg‚É•ÏŠ·‚µ‚Ä•Ô‚·
        List<Prediction> predictionsList = new();
        predictionsList.AddRange(predictions);
        return predictionsList;
    }
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

//**************************************************
//テスト用の麻雀牌認識結果を提供するクラス
//**************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DynamicSceneManagerHelper.SceneSnapshot;

public class Mock_TileData :IPredictions
{
    private Predictions _predictions;

    List<Prediction> IPredictions.GetAllPredictions()
    {
        if (_predictions == null)
        {
            Debug.LogError("predctionsの中身がありません");
        }
        return _predictions.GetAllPredictions();
    }

    /// <summary>
    /// jsonファイルから麻雀牌データを生成する
    /// </summary>
    /// <param name="jsonData"></param>
    public Mock_TileData(string jsonData)
    {
        _predictions = Convert_Json_To_Predictions(jsonData);
    }

    private Predictions Convert_Json_To_Predictions(string jsonData)
    {
        //「class」という変数名が使えないため「class_name」に置換する.
        string fixedText = jsonData.Replace("class", "class_name");
        //上の変換で「class_id」が「class_name_id」に変わってしまうので修正する.
        fixedText = fixedText.Replace("class_name_id", "class_id");

        //オブジェクトに変換.
        Predictions predictions = JsonUtility.FromJson<Predictions>(fixedText);
        return predictions;
    }
}

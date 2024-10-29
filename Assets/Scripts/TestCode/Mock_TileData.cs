//**************************************************
//�e�X�g�p�̖����v�F�����ʂ�񋟂���N���X
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
            Debug.LogError("predctions�̒��g������܂���");
        }
        return _predictions.GetAllPredictions();
    }

    /// <summary>
    /// json�t�@�C�����疃���v�f�[�^�𐶐�����
    /// </summary>
    /// <param name="jsonData"></param>
    public Mock_TileData(string jsonData)
    {
        _predictions = Convert_Json_To_Predictions(jsonData);
    }

    private Predictions Convert_Json_To_Predictions(string jsonData)
    {
        //�uclass�v�Ƃ����ϐ������g���Ȃ����߁uclass_name�v�ɒu������.
        string fixedText = jsonData.Replace("class", "class_name");
        //��̕ϊ��Łuclass_id�v���uclass_name_id�v�ɕς���Ă��܂��̂ŏC������.
        fixedText = fixedText.Replace("class_name_id", "class_id");

        //�I�u�W�F�N�g�ɕϊ�.
        Predictions predictions = JsonUtility.FromJson<Predictions>(fixedText);
        return predictions;
    }
}

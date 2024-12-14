//***************************
//LUTをシーンとパススルー両方に適用させるクラス.
//***************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LUTController : MonoBehaviour
{
    private Material _lutMaterial;
    private const string _weightPropertyName = "_weight";
    private const string _lutPropertyName = "_LUTTexture";
    private OVRPassthroughLayer _ovrPassthroughLayer;
    [SerializeField] private float _weight = 0;
    [SerializeField] private Texture2D _defaultLut;

    OVRPassthroughColorLut _latestLut;  //LUTの強さ単体では変更できないので現在のLUTを記憶しておく.

    private void Start()
    {
        //初期化.
        _ovrPassthroughLayer = FindObjectOfType<OVRPassthroughLayer>();
        _lutMaterial = Addressables.LoadAssetAsync<Material>("Material_LUT").WaitForCompletion();
        SetLut(_defaultLut, _weight);
    }

    private void Update()
    {
        SetLut(_defaultLut, _weight);
    }

    public void SetLut(Texture2D lutTex, float weight)
    {
        weight = Mathf.Clamp01(weight);
        OVRPassthroughColorLut ovrLut;
        //新しいテクスチャが無かったら最後に適用したLUTを設定する.
        if (lutTex == null)
        {
            if (_latestLut != null)
            {
                ovrLut = _latestLut;
            }
            else
            {
                return;
            }
        }
        else
        {
            ovrLut = new OVRPassthroughColorLut(lutTex,false);
            _latestLut = ovrLut;
        }

        //適用.
        _ovrPassthroughLayer.SetColorLut(ovrLut, weight);
        if (lutTex != null)
        {
            _lutMaterial.SetTexture(_lutPropertyName, lutTex);
        }
        _lutMaterial.SetFloat(_weightPropertyName, weight);
    }

    public void SetLutWeight(float weight)
    {
        if (_latestLut == null)
        {
            return;
        }
        weight = Mathf.Clamp01(weight);
        _ovrPassthroughLayer.SetColorLut(_latestLut, weight);
        _lutMaterial.SetFloat(_weightPropertyName, weight);
    }
}

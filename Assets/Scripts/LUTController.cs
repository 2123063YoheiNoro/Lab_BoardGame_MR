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
    private const string _weightPropertyName = "_weight";   //weightの変数名
    private const string _lutPropertyName = "_LUTTexture";  //lutテクスチャの変数名
    private OVRPassthroughLayer _ovrPassthroughLayer;

    [SerializeField] private float _weight = 0; //LUTの強さ.
    [SerializeField] private Texture2D _defaultLut; //初期のLUT.

    private OVRPassthroughColorLut _latestLut;  //LUTの強さ単体では変更できないので現在のLUTを記憶しておく.
    private Coroutine _latestCoroutine; //weightの変化に使うコルーチン(停止用)
    [SerializeField] private float ChangeSpeed = 1; //weightの変化の早さ

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
        _weight = weight;
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
            ovrLut = new OVRPassthroughColorLut(lutTex, false);
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
        _weight = weight;
        _ovrPassthroughLayer.SetColorLut(_latestLut, weight);
        _lutMaterial.SetFloat(_weightPropertyName, weight);
    }

    /// <summary>
    /// lutの強さを滑らかに変化させる.
    /// </summary>
    /// <param name="weight"></param>
    public void SetLutWeightSmooth(float weight)
    {
        if (_latestCoroutine != null)
        {
            StopCoroutine(_latestCoroutine);
        }

        StartCoroutine(SmoothChange(weight));
    }

    private IEnumerator SmoothChange(float targetValue)
    {
        targetValue = Mathf.Clamp01(targetValue);

        //weightを線形補間でゆっくり動かす.
        while (Mathf.Abs(_weight - targetValue) > 0.1f)
        {
            _weight = Mathf.Lerp(_weight, targetValue, Time.deltaTime * ChangeSpeed);
            SetLutWeight(_weight);
            yield return null;
        }

        SetLutWeight(targetValue);

        //yield returnが発生しない可能性があるのでここで実行.
        yield return null;
    }
}

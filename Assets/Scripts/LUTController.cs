//***************************
//LUT���V�[���ƃp�X�X���[�����ɓK�p������N���X.
//***************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LUTController : MonoBehaviour
{
    private Material _lutMaterial;
    private const string _weightPropertyName = "_weight";   //weight�̕ϐ���
    private const string _lutPropertyName = "_LUTTexture";  //lut�e�N�X�`���̕ϐ���
    private OVRPassthroughLayer _ovrPassthroughLayer;

    [SerializeField] private float _weight = 0; //LUT�̋���.
    [SerializeField] private Texture2D _defaultLut; //������LUT.

    private OVRPassthroughColorLut _latestLut;  //LUT�̋����P�̂ł͕ύX�ł��Ȃ��̂Ō��݂�LUT���L�����Ă���.
    private Coroutine _latestCoroutine; //weight�̕ω��Ɏg���R���[�`��(��~�p)
    [SerializeField] private float ChangeSpeed = 1; //weight�̕ω��̑���

    private void Start()
    {
        //������.
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
        //�V�����e�N�X�`��������������Ō�ɓK�p����LUT��ݒ肷��.
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

        //�K�p.
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
    /// lut�̋��������炩�ɕω�������.
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

        //weight����`��Ԃł�����蓮����.
        while (Mathf.Abs(_weight - targetValue) > 0.1f)
        {
            _weight = Mathf.Lerp(_weight, targetValue, Time.deltaTime * ChangeSpeed);
            SetLutWeight(_weight);
            yield return null;
        }

        SetLutWeight(targetValue);

        //yield return���������Ȃ��\��������̂ł����Ŏ��s.
        yield return null;
    }
}

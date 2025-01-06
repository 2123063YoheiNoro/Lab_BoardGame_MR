using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandConfigController : MonoBehaviour
{
    [SerializeField] private HandScoreModel _scoreModel;
    private HandConfig _handConfig;

    private bool is_tsumo = false;
    private bool is_riichi = false;
    private bool is_ippatsu = false;
    private bool is_rinshan = false;
    private bool is_chankan = false;
    private bool is_haitei = false;
    private bool is_houtei = false;
    private bool is_daburu_riichi = false;
    private bool is_nagashi_mangan = false;
    private bool is_tenhou = false;
    private bool is_renhou = false;
    private bool is_chiihou = false;
    private bool is_open_riichi = false;

    [SerializeField] private TMP_Text is_tsumo_text;
    [SerializeField] private TMP_Text is_riichi_text;
    [SerializeField] private TMP_Text is_ippatsu_text;
    [SerializeField] private TMP_Text is_rinshan_text;
    [SerializeField] private TMP_Text is_chankan_text;
    [SerializeField] private TMP_Text is_haitei_text;
    [SerializeField] private TMP_Text is_houtei_text;
    [SerializeField] private TMP_Text is_daburu_riichi_text;
    [SerializeField] private TMP_Text is_nagashi_mangan_text;
    [SerializeField] private TMP_Text is_tenhou_text;
    [SerializeField] private TMP_Text is_renhou_text;
    [SerializeField] private TMP_Text is_chiihou_text;
    [SerializeField] private TMP_Text is_open_riichi_text;


    private void Start()
    {
        _handConfig = _scoreModel._handConfig;
        InitializeTextFields();
    }

    // **TMP_Text �̏�����**
    private void InitializeTextFields()
    {
        is_tsumo_text.text = "�c�� OFF";
        is_riichi_text.text = "���[�` OFF";
        is_ippatsu_text.text = "�ꔭ OFF";
        is_rinshan_text.text = "���J�� OFF";
        is_chankan_text.text = "���� OFF";
        is_haitei_text.text = "�C�� OFF";
        is_houtei_text.text = "�͒� OFF";
        is_daburu_riichi_text.text = "�_�u�����[�` OFF";
        is_nagashi_mangan_text.text = "�������� OFF";
        is_tenhou_text.text = "�V�a OFF";
        is_renhou_text.text = "�l�a OFF";
        is_chiihou_text.text = "�n�a OFF";
        is_open_riichi_text.text = "�I�[�v�����[�` OFF";
    }

    // **�ėp�I�ȃg�O���֐�**
    private void ToggleFlag(ref bool flag, TMP_Text textComponent, string label, ref bool handConfigFlag)
    {
        flag = !flag;
        handConfigFlag = flag;
        textComponent.text = $"{label} {(flag ? "ON" : "OFF")}";
    }

    // **���ׂẴg�O���֐�**
    public void Toggle_is_tsumo() => ToggleFlag(ref is_tsumo, is_tsumo_text, "�c��", ref _handConfig.is_tsumo);
    public void Toggle_is_riichi() => ToggleFlag(ref is_riichi, is_riichi_text, "���[�`", ref _handConfig.is_riichi);
    public void Toggle_is_ippatsu() => ToggleFlag(ref is_ippatsu, is_ippatsu_text, "�ꔭ", ref _handConfig.is_ippatsu);
    public void Toggle_is_rinshan() => ToggleFlag(ref is_rinshan, is_rinshan_text, "���J��", ref _handConfig.is_rinshan);
    public void Toggle_is_chankan() => ToggleFlag(ref is_chankan, is_chankan_text, "����", ref _handConfig.is_chankan);
    public void Toggle_is_haitei() => ToggleFlag(ref is_haitei, is_haitei_text, "�C��", ref _handConfig.is_haitei);
    public void Toggle_is_houtei() => ToggleFlag(ref is_houtei, is_houtei_text, "�͒�", ref _handConfig.is_houtei);
    public void Toggle_is_daburu_riichi() => ToggleFlag(ref is_daburu_riichi, is_daburu_riichi_text, "�_�u�����[�`", ref _handConfig.is_daburu_riichi);
    public void Toggle_is_nagashi_mangan() => ToggleFlag(ref is_nagashi_mangan, is_nagashi_mangan_text, "��������", ref _handConfig.is_nagashi_mangan);
    public void Toggle_is_tenhou() => ToggleFlag(ref is_tenhou, is_tenhou_text, "�V�a", ref _handConfig.is_tenhou);
    public void Toggle_is_renhou() => ToggleFlag(ref is_renhou, is_renhou_text, "�l�a", ref _handConfig.is_renhou);
    public void Toggle_is_chiihou() => ToggleFlag(ref is_chiihou, is_chiihou_text, "�n�a", ref _handConfig.is_chiihou);
    public void Toggle_is_open_riichi() => ToggleFlag(ref is_open_riichi, is_open_riichi_text, "�I�[�v�����[�`", ref _handConfig.is_open_riichi);
}

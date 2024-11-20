using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandScoreView:MonoBehaviour
{
    [SerializeField] TMP_Text cost;
    [SerializeField] TMP_Text han;
    [SerializeField] TMP_Text fu;
    [SerializeField] TMP_Text yaku;

    private void Start()
    {
        cost.text = "";
        han.text = "";
        fu.text = "";
        yaku.text = "";
    }

    public void UpdateText(HandResponse handResponse)
    {
        Debug.Log("text updated");
        int totalCost = handResponse.cost_main + handResponse.cost_aditional * 2;
        cost.text = totalCost.ToString();
        han.text = handResponse.han.ToString();
        fu.text = handResponse.fu.ToString();
        yaku.text = handResponse.yaku;
    }
}

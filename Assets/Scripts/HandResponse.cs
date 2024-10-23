using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HandResponse
{
    public int cost_main;
    public int cost_aditional;
    public int han;
    public int fu;
    public List<string> yaku;

    public HandResponse(int cost_main = 0, int cost_aditional = 0, int han = 0, int fu = 0, List<string> yaku = null)
    {
        this.cost_main = cost_main;
        this.cost_aditional = cost_aditional;
        this.han = han;
        this.fu = fu;
        this.yaku = yaku;
    }
}

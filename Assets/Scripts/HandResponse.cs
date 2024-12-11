using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HandResponse : IEquatable<HandResponse>
{
    public int cost_main;
    public int cost_aditional;
    public int han;
    public int fu;
    public string yaku;

    public HandResponse(int cost_main = 0, int cost_aditional = 0, int han = 0, int fu = 0, dynamic yaku = null)
    {
        this.cost_main = cost_main;
        this.cost_aditional = cost_aditional;
        this.han = han;
        this.fu = fu;
        this.yaku = yaku;
    }

    //reactiveProperty用に定義しておく
    public bool Equals(HandResponse other)
    {
        //全部のメンバ変数の値が同じなら同じものとして扱う
        return 
            this.cost_main == other.cost_main &&
            this.cost_aditional == other.cost_aditional &&
            this.han == other.han &&
            this.fu == other.fu &&
            this.yaku == other.yaku;
    }
}

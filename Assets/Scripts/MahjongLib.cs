using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public static class MahjongLib
{
    [DllImport("MahjongNativePlugin")]
    public static extern int GetNumber();
}

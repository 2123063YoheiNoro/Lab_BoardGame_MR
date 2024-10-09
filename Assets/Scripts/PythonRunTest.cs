using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using System;

namespace UnityPython
{

    [DefaultExecutionOrder(-1)]
    public class PythonRunTest : MonoBehaviour
    {
        dynamic np;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("python test");
            RunMahjonScript();
        }

        private void RunMahjonScript()
        {
            np = Py.Import("numpy");
            float tmp = np.cos(np.pi * 2);
            Debug.Log("np.cos(np.pi * 2) =" + tmp);

            using dynamic mj_tiles = Py.Import("mahjong.tile");
            using dynamic mj_calculate = Py.Import("mahjong.hand_calculating.hand");
            using dynamic mj_shanten = Py.Import("mahjong.shanten");

            var calculator = mj_calculate.HandCalculator();
            var shantenCalculator = mj_shanten.Shanten();

            var tiles = mj_tiles.TilesConverter.string_to_136_array("234555", "555", "22555");
            var win_tiles = mj_tiles.TilesConverter.string_to_136_array("", "", "5")[0];
            var melds = "";
            var dora = mj_tiles.TilesConverter.string_to_136_array("4", "", "");
            var config = "";
            var result = calculator.estimate_hand_value(tiles, win_tiles, melds, dora, config);
            Debug.Log(result.yaku+"\n"+result.han+"–|"+result.fu+"•„"+"\n"+result.cost["main"]+","+result.cost["additional"]);

            var tiles_14 = mj_tiles.TilesConverter.string_to_34_array("13569", "123459", "443");
            var tiles136= mj_tiles.TilesConverter.string_to_136_array("13569", "123459", "443");
            var tiles136_d = mj_tiles.TilesConverter.to_136_array(tiles_14);
            var shantenresult = shantenCalculator.calculate_shanten(tiles_14);
            Debug.Log(shantenresult);
        }
    }
}
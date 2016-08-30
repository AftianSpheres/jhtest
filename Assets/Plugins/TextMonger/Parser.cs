using UnityEngine;
using System.Collections.Generic;

namespace TextMonger
{
    enum EscCode
    {
        None,
        blockEnd,
        btn_DPDown,
        btn_DPUp,
        btn_DPLeft,
        btn_DPRight,
        btn_snesB,
        btn_snesA,
        btn_snesX,
        btn_snesY,
        btn_start,
        btn_select,
        lstick_neutral,
        lstick_down,
        lstick_up,
        lstick_left,
        lstick_right,
        rstick_neutral,
        rstick_down,
        rstick_up,
        rstick_left,
        rstick_right,
        btn_l1,
        btn_l2,
        btn_l3,
        btn_r1,
        btn_r2,
        btn_r3
    }

    public static class Parser
    {
        public static TextBlock[] ParseStringIntoBlockArray(string input)
        {
            List<TextBlock> output = new List<TextBlock>();
            return output.ToArray();
        }
    }
}



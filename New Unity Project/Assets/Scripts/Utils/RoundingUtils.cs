using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RoundingUtils
{

    public static string GetShorthand(float number)
    {
        foreach (KeyValuePair<float, string> p in shorthandDic)
        {
            if (number >= Mathf.Pow(10, 15) && number >= p.Key && number <= p.Key * 10)
            {//Scientific Notation
                return Math.Round(number / p.Key, 2).ToString() + p.Value;
            }
            else if (number <= Mathf.Pow(10, 15) && number >= p.Key && number <= p.Key * 1000)
            {// regular notation
                return Math.Round(number / p.Key, 2).ToString() + p.Value;
            }
        }
        return number.ToString();
    }

    public RoundingUtils()
    {
        InitShorthandDic();
    }
    public static Dictionary<float, string> shorthandDic;
    private void InitShorthandDic()
    {
        shorthandDic = new Dictionary<float, string>();
        shorthandDic.Add(Mathf.Pow(10, 3), " K");
        shorthandDic.Add(Mathf.Pow(10, 6), " M");
        shorthandDic.Add(Mathf.Pow(10, 9), " B");
        shorthandDic.Add(Mathf.Pow(10, 12), " T");
        shorthandDic.Add(Mathf.Pow(10, 15), " e+15");
        shorthandDic.Add(Mathf.Pow(10, 16), " e+16");
        shorthandDic.Add(Mathf.Pow(10, 17), " e+17");
        shorthandDic.Add(Mathf.Pow(10, 18), " e+18");
        shorthandDic.Add(Mathf.Pow(10, 21), " e+21");
        shorthandDic.Add(Mathf.Pow(10, 24), " e+24");
    }
}

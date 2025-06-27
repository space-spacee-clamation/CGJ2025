using System;
using System.Collections.Generic;
using UnityEngine;
public enum PowerEnum
{
    Null, //没使用
    Water,
    Fire,
}
public class PowerUseAbleComponent : MonoBehaviour
{
    protected Dictionary<PowerEnum, Action> powerDic = new Dictionary<PowerEnum, Action>();
    public bool CouldUsePower(PowerEnum eEnum)
    {
        return powerDic.ContainsKey(eEnum);
    }
    public void UsePower(PowerEnum eEnum)
    {
        if (powerDic.TryGetValue(eEnum, out Action action)) action.Invoke();
    }
    public void AddCallBack(PowerEnum key, Action pw)
    {
        if (powerDic.ContainsKey(key))
        {
            powerDic[key] += pw;
        }
        else
        {
            powerDic.Add(key, pw);
        }
    }
    public void RemoveCallBack(PowerEnum key, Action pw)
    {
        if (powerDic.ContainsKey(key))
        {
            powerDic[key] -= pw;
        }
    }
}
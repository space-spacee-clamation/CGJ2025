using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制元素之力！
/// </summary>
public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance;
    public Action<PowerEnum> OnChangePower;
    private int nowIndex = 0;
    public PowerEnum NowPower{get; private set;}
    private List<PowerEnum> _enumLists=new List<PowerEnum>();
    private void Awake()
    {
        if (Instance!=null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        foreach (var aValue in    Enum.GetValues(typeof(PowerEnum)))
        {
            _enumLists.Add((PowerEnum)aValue);
        }
        //TODO: 临时测试用
        OnChangePower += (aPower) =>
        {
            Debug.Log($"ChangePower: {NowPower.ToString()}");
        };
    }
    public void ChangePower(int num)
    {
        nowIndex = (nowIndex + num) % _enumLists.Count;
        NowPower= _enumLists[nowIndex];
        OnChangePower?.Invoke(NowPower);
    }
}

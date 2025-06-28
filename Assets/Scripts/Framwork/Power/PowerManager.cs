using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///     玩家控制元素之力！
/// </summary>
public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance;
    private readonly List<PowerEnum> _enumLists = new List<PowerEnum>();
    private int nowIndex  ;
    public Action<PowerEnum> OnChangePower;
    public PowerEnum NowPower { get; private set; }
    private void OnEnable()
    {
        Destroy(Instance);
        Instance = this;
    }
    private void Start()
    {
        foreach (object aValue in    Enum.GetValues(typeof(PowerEnum)))
        {
            _enumLists.Add((PowerEnum)aValue);
        }
        //TODO: 临时测试用
        OnChangePower += aPower => {
            Debug.Log($"ChangePower: {NowPower.ToString()}");
        };
    }
    public void ChangePower(int num)
    {
        nowIndex = (nowIndex + num) % _enumLists.Count;
        NowPower = _enumLists[nowIndex];
        OnChangePower?.Invoke(NowPower);
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance;
    private readonly List<PowerEnum> _enumLists = new List<PowerEnum>();
    private int nowIndex  ;
    public Action<PowerEnum> OnChangePower;
    private readonly Dictionary<PowerEnum, int> powerDic = new Dictionary<PowerEnum, int>();
    public PowerEnum NowPower { get; private set; }
    private void Start()
    {
        foreach (object aValue in    Enum.GetValues(typeof(PowerEnum)))
        {
            if (!aValue.Equals(PowerEnum.Null))
                _enumLists.Add((PowerEnum)aValue);
        }
        
    }
    private void OnEnable()
    {
        Destroy(Instance);
        Instance = this;
        OnChangePower += dd => {
            Debug.Log($"nowPower: {dd} has {(powerDic.ContainsKey(dd) ? powerDic[dd] : 0)}");
        };
    }
    public void ChangePower(int num)
    {
        nowIndex = (nowIndex + num) % _enumLists.Count;
        NowPower = _enumLists[nowIndex];
        OnChangePower?.Invoke(NowPower);
    }
    public void ChangePower(PowerEnum num)
    {
        NowPower = num;
        if (_enumLists.Contains( num))
        {
            nowIndex = _enumLists.IndexOf(num);
        }
        OnChangePower?.Invoke(NowPower);
    }
    public void GetPower(PowerEnum ty, int num)
    {
        if (powerDic.ContainsKey( ty))
        {
            powerDic[ ty] += num;
        }
        else
        {
            powerDic.Add( ty, num);
        }
    }
    public bool TryUsePower(PowerEnum pe)
    {
        if (powerDic.ContainsKey(pe))
        {
            if (powerDic[pe] > 0)
            {
                powerDic[pe]--;
                switch (pe)
                {
                    case PowerEnum.Water:
                        AudioManager.Instance.PlayOnce("CallWater");
                        break;
                    case PowerEnum.Fire:
                        AudioManager.Instance.PlayOnce("CallFire");

                        break;
                }
                return true;
            }
        }
        return false;
    }
}
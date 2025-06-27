
using System;
using UnityEngine;
public enum PowerEnum
{
    Water,
     Fire
}
public interface IPowerUseAble
{
    public bool CouldUsePower(PowerEnum eEnum);
    public void UsePower(PowerEnum eEnum);
    public Action<PowerEnum> OnPowerUse { get; set; }
}

public class PowerUseAbleComponent : MonoBehaviour, IPowerUseAble
{

    public bool CouldUsePower(PowerEnum eEnum)
    {
        return false;
    }
    public void UsePower(PowerEnum eEnum)
    {
       
    }
    public Action<PowerEnum> OnPowerUse {
        get;
        set;
    }
}
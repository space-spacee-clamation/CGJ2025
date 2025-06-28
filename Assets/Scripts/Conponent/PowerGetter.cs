using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
[Serializable]
public class PowerGetSetting
{
    public PowerEnum _enum;
    public int num;
}
public class PowerGetter : ABaseControlAble
{
    [SerializeField] private List<PowerGetSetting> _settings;
    private bool drity ;
    [SerializeField] private SpriteRenderer light;
    protected override void Start()
    {
        base.Start();
        OnControl += () => {
            if (!drity)
            {
                BoPian();
                drity = true;
            }
        };
    }
    public override void Input(ControlType type, object param)
    {
    }
    protected void BoPian()
    {
        StartCoroutine(ChangeTime());
    }
    public override bool ControllAble()
    {
        return !drity;
    }
    protected IEnumerator ChangeTime()
    {
        foreach (PowerGetSetting setting in _settings)
        {
            PowerManager.Instance.GetPower(setting._enum, setting.num);
        }
        //TODO:特效动画
        yield return new WaitForSeconds(ConstClass.POWER_GET_TIME);
        LeaveControl();
    }
}
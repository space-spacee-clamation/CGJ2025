using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private SpriteRenderer light;
    private bool drity ;
    protected override void Start()
    {
        base.Start();
        OnControl += () => {
            if (!drity)
            {
                BoPian();
                drity = true;
                DialogManager.Instance.ShowDialog("感受到了抛瓦！", transform.position);
                AudioManager.Instance.PlayOnce("GetPower");
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
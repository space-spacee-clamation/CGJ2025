using ControlAble;
using UnityEngine;
public class PlayerComponent : ANormalMove
{
    [SerializeField] protected Eye _eye;

    public override IControlAble GetFacingObj()
    {
        return   _eye.GetFacingObjComponent<IControlAble>();
    }
    protected override void OnStart()
    {
        GameManager.Instance.SubPlayer(this);
        _eye = GetComponentInChildren<Eye>();
    }
    protected override void Fire()
    {
        OnFire();
    }
    protected override void GetOtherInput(ControlType type, object o)
    {
        if (type == ControlType.Next)
        {
            PowerManager.Instance.ChangePower(1);
        }
    }

    private void OnFire()
    {
        if (PowerManager.Instance.NowPower.Equals(PowerEnum.Null))
        {
            IControlAble res = _eye.GetFacingObjComponent<IControlAble>();
            PlayerController.Instance.SetControlAble(res);
        }
        else
        {
            PowerUseAbleComponent res = _eye.GetFacingObjComponent<PowerUseAbleComponent>();
            res?.UsePower(PowerManager.Instance.NowPower);
        }
    }

}
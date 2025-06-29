using ControlAble;
using UnityEngine;
public class PlayerComponent : ANormalMove
{
    [SerializeField] protected Eye _eye;
    [SerializeField] private Animator _animator;
    private void OnEnable()
    {
        GameManager.Instance.SubPlayer(this,transform);
    }
    protected override void OnStart()
    {
        _eye = GetComponentInChildren<Eye>();
    }
    protected override void Fire()
    {
        OnFire();
    }
    protected override void OnUpdate()
    {
        _animator.SetBool("isJump", !_feet.IsGround);
        _animator.SetBool("isRun", _feet.IsGround &&  Mathf.Abs(rigidbody2D.velocity.x) > 0.1f);
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
        if (!PowerManager.Instance.NowPower.Equals(PowerEnum.Null))
        {
            PowerUseAbleComponent res = _eye.GetFacingObjComponent<PowerUseAbleComponent>();
            if (res != null)
                res.UsePower(PowerManager.Instance.NowPower);
            else
            {
                IControlAble resC = _eye.GetFacingObjComponent<IControlAble>();
                if (resC != null)
                {
                    PowerManager.Instance.ChangePower(PowerEnum.Null);
                    PlayerController.Instance.SetControlAble(resC);
                }
            }
        }
        else if (PowerManager.Instance.NowPower.Equals(PowerEnum.Null))
        {
            IControlAble res = _eye.GetFacingObjComponent<IControlAble>();
            PlayerController.Instance.SetControlAble(res);
        }

    }
}
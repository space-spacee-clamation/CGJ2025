using System;
using UnityEngine;
public enum ControlType
{
    UP,
    Right,
    Left,
    Down,
    Jump,
    Fire,
    Next, //切换元素
}
public interface IControlAble
{
    public Action OnControl { get; }
    public Action OnRelease { get; }
    public void Input(ControlType  type, object  param);
    public bool ControllAble();
}
public abstract class ABaseControlAble : MonoBehaviour, IControlAble
{
    protected virtual void Start()
    {
        OnControl += () => {
            PlayerController.Instance.VirtualCamera.Follow = transform;
        };
    }
    public Action OnControl {
        get;
        protected set;
    }
    public Action OnRelease {
        get;
        protected set;
    }
    public abstract void Input(ControlType type, object param);
    public virtual bool ControllAble()
    {
        return true;
    }
    protected void LeaveControl()
    {
        PlayerController.Instance.RemoveControlAble();
    }
}
using ControlAble;
using UnityEngine;
public class StoneComponent : ANormalMove
{
    protected override void Control()
    {
        base.Control();
        AudioManager.Instance.PlayOnce("StoneJiaoHu");
        DialogManager.Instance.ShowDialog("saki，移动", transform.position);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(Mathf.Abs( rigidbody2D.velocity.x)<0.3f)
            AudioManager.Instance.StopSfx("StoneMove");

    }
    protected override void HorizontalMove(float value)
    {
        base.HorizontalMove(value);
        AudioManager.Instance.PlayOnce("StoneMove");
    }
}
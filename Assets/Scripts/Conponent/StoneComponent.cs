using ControlAble;
public class StoneComponent : ANormalMove
{
    protected override void Control()
    {
        base.Control();
        AudioManager.Instance.PlayOnce("StoneIn");
    }
    protected override void HorizontalMove(float value)
    {
        base.HorizontalMove(value);
        AudioManager.Instance.PlayOnce("StoneMove");
    }
}
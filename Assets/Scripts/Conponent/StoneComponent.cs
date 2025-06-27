using ControlAble;
public class StoneComponent : ANormalMove
{
    public override IControlAble GetFacingObj()
    {
        return GameManager.Instance.GetPlayer();
    }
}
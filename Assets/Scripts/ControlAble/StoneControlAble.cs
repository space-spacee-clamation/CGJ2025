    using ControlAble;
    public class StoneControlAble : ANormalMove
    {
        public override IControlAble GetFacingObj()
        {
            return GameManager.Instance.GetPlayer();
        }
    }

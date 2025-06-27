    using ControlAble;
    public class Stone : ANormalMove
    {

        public override IControlAble GetFacingObj()
        {
            return GameManager.Instance.GetPlayer();
        }
    }

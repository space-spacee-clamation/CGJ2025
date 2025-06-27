    using ControlAble;
    using UnityEngine;
    public class PlayerControlAble : ANormalMove
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

    }

    using ControlAble;
    using UnityEngine;
    public class Player : ANormalMove
    {
        [SerializeField] protected Eye _eye;
        
        
        public override IControlAble GetFacingObj()
        {
            return   _eye.GetFacingObjControlAble();
        }
        protected override void OnStart()
        {
            GameManager.Instance.SubPlayer(this);
            _eye = GetComponentInChildren<Eye>();
        }

    }

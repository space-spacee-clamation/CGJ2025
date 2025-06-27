
    using System;
    using UnityEngine;
    /// <summary>
    /// 玩家控制器
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private IControlAble _controlAble;
        public void Start()
        {
            _controlAble?.OnControl?.Invoke();
        }
        private void Update()
        {
            InputUpdate();
        }
        public void RemoveControlAble()
        {
            _controlAble?.OnRelease?.Invoke();
            _controlAble = GameManager.Instance.GetPlayer();
            _controlAble?.OnControl?.Invoke();
        }
        public void SetControlAble(IControlAble controlAble)
        {
            if(controlAble==_controlAble) return;
            if (controlAble != null) controlAble.OnRelease?.Invoke();
            _controlAble = controlAble;
            _controlAble?.OnControl?.Invoke();
        }
        public void InputUpdate()
        {
            //默认值
            if(_controlAble==null) _controlAble=GameManager.Instance.GetPlayer();
            
           var tempHorizontal= Input.GetAxis("Horizontal");
           if (tempHorizontal != 0)
           {
               _controlAble.Input(tempHorizontal > 0 ? ControlType.Right : ControlType.Left,tempHorizontal);
           }
           var tempVertical = Input.GetAxis("Vertical");
           if (tempVertical != 0)
           {
               _controlAble.Input(tempVertical > 0 ? ControlType.UP : ControlType.Down,tempVertical);
           }
           if (Input.GetKeyDown(KeyCode.Space))
           {
               _controlAble.Input(ControlType.Jump,null);
           }
           if (Input.GetKeyDown(KeyCode.F))
           {
               //??怎么去做附身呢？
               SetControlAble(_controlAble.GetFacingObj());
           }
           if (Input.GetKeyDown(KeyCode.Q))
           {
               RemoveControlAble();
           }
           //TODO: 其它交互
        }
    }

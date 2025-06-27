
    using System;
    using UnityEngine;
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private IControlAble _player;
        private void Awake()
        {
            Instance = this;
        }
        public void SubPlayer(IControlAble controlAble)
        {
            _player = controlAble;
        }
        public IControlAble GetPlayer()
        {
            if(_player!=null)
            return _player;
            throw new Exception("未注册玩家");
        }
    }

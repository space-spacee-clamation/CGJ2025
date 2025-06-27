using System;
using UnityEngine;
namespace ControlAble
{
    public class TreeComponent : ABaseControlAble
    {
        [SerializeField] float growUpSpeed;
        [SerializeField] private float maxHight;
        [SerializeField] private float minHight;

        [SerializeField] private PowerUseAbleComponent pwComponent;

        private void Awake()
        {
            pwComponent=GetComponent<PowerUseAbleComponent>();
            pwComponent.AddCallBack(PowerEnum.Water,GetWater);
            pwComponent.AddCallBack(PowerEnum.Fire,GetFire);
        }
        private void GetFire()
        {
            //TODO: 待确定
        }
        private void GetWater()
        {
            //TODO: 具体确定
            maxHight = 3;
        }
        

        private void Update()
        {
            if (isGrowUp && transform.localScale.y<maxHight)
            {
                transform.localScale+=growUpSpeed*Time.deltaTime*Vector3.up;
            }
            else if(!isGrowUp && transform.localScale.y>minHight)
            {
                transform.localScale+=growUpSpeed*Time.deltaTime*Vector3.down;
            }
        }
        public override void Input(ControlType type, object param)
        {
            switch (type)
            {
                case ControlType.UP:
                    GrowUp((float)param);
                    break;
                case ControlType.Down:
                    GrowDown((float)param);
                    break;
                case ControlType.Fire:
                    LeaveControl();
                    break;
            }
        }
        private void GrowDown(float f)
        {
            isGrowUp = false;
        }
        bool isGrowUp=false;
        private void GrowUp(float o)
        {
            isGrowUp = true;
        }
        public override IControlAble GetFacingObj()
        {
            return GameManager.Instance.GetPlayer();
        }
    }
}
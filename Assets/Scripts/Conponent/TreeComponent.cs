using System.Collections.Generic;
using UnityEngine;
namespace ControlAble
{
    public class TreeComponent : ANormalMove , IChangeWithTime
    {
        [SerializeField] private float growUpSpeed;
        [SerializeField] private float maxHight;
        [SerializeField] private float minHight;

        [SerializeField] private PowerUseAbleComponent pwComponent;
        private bool couldWater;
        private bool isGrowUp ;
        private float nowHight;

        /// <summary>
        /// 三个状态
        /// </summary>
        private List<Sprite> _sprites;
        private SpriteRenderer renderer;
        private Transform colliderTrans;
        private void Awake()
        {
            pwComponent = GetComponent<PowerUseAbleComponent>();
            pwComponent.AddCallBack(PowerEnum.Water, GetWater);
            pwComponent.AddCallBack(PowerEnum.Fire, GetFire);
            nowHight = minHight;
        }


        private void Update()
        {
            switch (nowHight)
            {
                case 1 :
                    renderer.sprite = _sprites[0];
                    colliderTrans.localScale= Vector3.one;
                    break;
                case 2:
                    renderer.sprite = _sprites[1];
                    colliderTrans.localScale= Vector3.one+Vector3.up;
                    break;
                case 3:
                    renderer.sprite = _sprites[2];
                    colliderTrans.localScale= Vector3.one+Vector3.up*2;
                    break;
            }
        }
        public void ChangeWithWeather(GameTimeEnum time)
        {
            switch (time)
            {
                case GameTimeEnum.Day:
                    couldWater = true;
                    break;
                case GameTimeEnum.Night:
                    couldWater = false;
                    break;
            }
        }
        private void GetFire()
        {
            nowHight -= 1;
            AudioManager.Instance.PlayOnce("FireTree");
            if (nowHight < minHight) nowHight = minHight;
        }
        private void GetWater()
        {
            if (couldWater)
            {
                nowHight += 1;
                AudioManager.Instance.PlayOnce("WaterTree");
                if (nowHight > maxHight) nowHight = maxHight;
            }
            else
            {
                DialogManager.Instance.ShowDialog("只能在白天浇水", transform.position);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
namespace ControlAble
{
    public class TreeComponent : ANormalMove , IChangeWithTime
    {
        [SerializeField] private float growUpSpeed=1f;
        [SerializeField] private int maxHight = 3;
        [SerializeField] private int minHight=1;

        [SerializeField] private PowerUseAbleComponent pwComponent;
        private bool couldWater=true;
        private bool isGrowUp ;
        private float nowHight;

        /// <summary>
        /// 三个状态
        /// </summary>
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private SpriteRenderer renderer;
       [SerializeField]  private Transform colliderTrans;
        private void Awake()
        {
            pwComponent.AddCallBack(PowerEnum.Water, GetWater);
            pwComponent.AddCallBack(PowerEnum.Fire, GetFire);
            nowHight = minHight;
        }
        protected override void OnStart()
        {
            base.OnStart();
            TimeController.Instance.SubObj(this);
        }

        override protected void OnUpdate()
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
                    colliderTrans.localScale= Vector3.one+Vector3.up*2f;
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
            DialogManager.Instance.ShowDialog("世界，咀嚼了我", transform.position);
            if (nowHight < minHight) nowHight = minHight;
        }
        private void GetWater()
        {
            if (couldWater)
            {
                nowHight += 1;
                AudioManager.Instance.PlayOnce("WaterTree");
                DialogManager.Instance.ShowDialog("小树苗，快长高", transform.position);
                Debug.Log("????");
                if (nowHight > maxHight) nowHight = maxHight;
            }
            else
            {
                DialogManager.Instance.ShowDialog("只能在白天浇水", transform.position);
            }
        }
    }
}
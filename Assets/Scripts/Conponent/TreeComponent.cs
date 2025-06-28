using UnityEngine;
namespace ControlAble
{
    public class TreeComponent : ANormalMove
    {
        [SerializeField] private float growUpSpeed;
        [SerializeField] private float maxHight;
        [SerializeField] private float minHight;
        private float nowHight;

        [SerializeField] private PowerUseAbleComponent pwComponent;
        private bool isGrowUp ;

        private void Awake()
        {
            pwComponent = GetComponent<PowerUseAbleComponent>();
            pwComponent.AddCallBack(PowerEnum.Water, GetWater);
            pwComponent.AddCallBack(PowerEnum.Fire, GetFire);
        }


        private void Update()
        {
            if (transform.localScale.y < nowHight-0.05f)
            {
                transform.localScale += growUpSpeed * Time.deltaTime * Vector3.up;
            }
            else if ( transform.localScale.y > minHight+0.05f)
            {
                transform.localScale += growUpSpeed * Time.deltaTime * Vector3.down;
            }
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
        protected override void VerticalMove(float value)
        {
            if (nowHight > 0)
            {
                GrowUp();
            }
            else
            {
                GrowDown();
            }
        }
        private void GrowDown()
        {
            nowHight -= 1;
            if (nowHight < minHight) nowHight = minHight;
        }
        private void GrowUp()
        {
            nowHight += 1;
            if (nowHight > maxHight) nowHight = maxHight;
        }
        public override IControlAble GetFacingObj()
        {
            return GameManager.Instance.GetPlayer();
        }
    }
}
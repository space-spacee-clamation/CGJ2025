using UnityEngine;
namespace ControlAble
{
    public class TreeComponent : ANormalMove
    {
        [SerializeField] private float growUpSpeed;
        [SerializeField] private float maxHight;
        [SerializeField] private float minHight;

        [SerializeField] private PowerUseAbleComponent pwComponent;
        private bool isGrowUp ;
        private float nowHight;

        private void Awake()
        {
            pwComponent = GetComponent<PowerUseAbleComponent>();
            pwComponent.AddCallBack(PowerEnum.Water, GetWater);
            pwComponent.AddCallBack(PowerEnum.Fire, GetFire);
            nowHight = minHight;
        }


        private void Update()
        {
            if (transform.localScale.y < nowHight - 0.05f)
            {
                transform.localScale += growUpSpeed * Time.deltaTime * Vector3.up;
            }
            if ( transform.localScale.y > nowHight + 0.05f)
            {
                transform.localScale += growUpSpeed * Time.deltaTime * Vector3.down;
            }
        }
        private void GetFire()
        {

        }
        private void GetWater()
        {

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
    }
}
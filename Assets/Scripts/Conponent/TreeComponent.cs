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
            nowHight -= 1;
            AudioManager.Instance.PlayOnce("FireTree");
            if (nowHight < minHight) nowHight = minHight;
        }
        private void GetWater()
        {
            nowHight += 1;
            AudioManager.Instance.PlayOnce("WaterTree");
            if (nowHight > maxHight) nowHight = maxHight;
        }
    }
}
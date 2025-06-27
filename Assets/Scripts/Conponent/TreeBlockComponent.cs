using System.Collections;
using UnityEngine;
public class TreeBlockComponent : MonoBehaviour
{
    [SerializeField]
    private PowerUseAbleComponent powerUseAbleComponent;

    /// <summary>
    ///     临时特效
    /// </summary>
    [SerializeField] private GameObject boom;
    private void Awake()
    {
        powerUseAbleComponent = GetComponent<PowerUseAbleComponent>();
        powerUseAbleComponent.AddCallBack(PowerEnum.Fire, GetFire);
        powerUseAbleComponent.AddCallBack(PowerEnum.Water, GetWater);
    }
    private void GetWater()
    {
        Debug.Log("AAAAAA");
    }
    private void GetFire()
    {
        Debug.Log("BBBBBBB");
        StartCoroutine(DestroySelf());
    }
    private IEnumerator DestroySelf()
    {
        GameObject bb = Instantiate(boom, transform.position, Quaternion.Euler(-90, 0, 0));
        bb.transform.SetParent(transform);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        //TODO: 燃起来了动画
    }
}
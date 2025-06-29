using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
public class TreeBlockComponent : MonoBehaviour
{
    [SerializeField]
    private PowerUseAbleComponent powerUseAbleComponent;

    [SerializeField] private GameObject leaveObj;
    [FormerlySerializedAs("pos")] [SerializeField] private Transform leavePos;
    /// <summary>
    ///     临时特效
    /// </summary>
    [SerializeField] private GameObject boom;
    [SerializeField] private Animator _animator;
    private void Awake()
    {
        powerUseAbleComponent = GetComponent<PowerUseAbleComponent>();
        powerUseAbleComponent.AddCallBack(PowerEnum.Fire, GetFire);
        powerUseAbleComponent.AddCallBack(PowerEnum.Water, GetWater);
    }
    private void GetWater()
    {
    }
    private void GetFire()
    {
        DialogManager.Instance.ShowDialog("让我们把藤蔓烧成灰", transform.position);
        StartCoroutine(DestroySelf());
    }
    private IEnumerator DestroySelf()
    {
        AudioManager.Instance.PlayOnce("FirePlant");
        // GameObject bb = Instantiate(boom, transform.position, Quaternion.Euler(-90, 0, 0));
        // bb.transform.SetParent(transform);
        _animator.Play("Burn");
        yield return new WaitForSeconds(1);
        Instantiate(leaveObj, leavePos.position, quaternion.identity);
        Destroy(gameObject);
        //TODO: 燃起来了动画
    }
}
using UnityEngine;
/// <summary>
///     获得视野前的物体的组件
/// </summary>
public class Eye : MonoBehaviour
{
    [SerializeField] private Transform eyePoint;
    [SerializeField] private float viewDis;
    [SerializeField] private LayerMask _mask;
    private IControlAble bodyAble;

    private void Awake()
    {
        bodyAble = gameObject.GetComponent<IControlAble>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePoint.position, eyePoint.position + transform.localScale.x * viewDis * Vector3.right);
    }
    public  T GetFacingObjComponent<T>()
    {
        RaycastHit2D[] res = Physics2D.RaycastAll(eyePoint.position, transform.localScale.x  * Vector3.right, viewDis, _mask);
        T minDisRes = default;
        float minDIS = float.MaxValue;
        T  bodyCo = gameObject.GetComponent<T>();
        foreach (RaycastHit2D result in res)
        {
            if (result.collider.TryGetComponent(out T able))
            {
                if (bodyCo != null && bodyCo.Equals(able)) continue;
                if (minDIS > result.distance)
                {
                    minDisRes = able;
                    minDIS = result.distance;
                }
            }
        }
        return minDisRes;
    }
}
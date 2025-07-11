using System;
using UnityEngine;
/// <summary>
///     获得视野前的物体的组件
/// </summary>
public class Eye : MonoBehaviour
{
    [SerializeField] private Transform eyePoint;
    [SerializeField] private float range;
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
        Gizmos.DrawLine(eyePoint.position, eyePoint.position + transform.localScale.x * viewDis * (Quaternion.Euler(0, 0, range) * Vector3.right));
        Gizmos.DrawLine(eyePoint.position, eyePoint.position + transform.localScale.x * viewDis * (Quaternion.Euler(0, 0, -range) * Vector3.right));
    }
    public  T GetFacingObjComponent<T>()
    {
        RaycastHit2D[] res = Physics2D.CircleCastAll(eyePoint.position, viewDis, MathF.Sign(transform.localScale.x) * Vector3.right, MathF.Cos( range / 360 * 2 * MathF.PI));
        T minDisRes = default;
        float minDIS = float.MaxValue;
        T  bodyCo = gameObject.GetComponent<T>();
        foreach (RaycastHit2D result in res)
        {
            float xDis = ((Vector3)result.point - eyePoint.position).x;
            if( MathF.Sign(xDis)==MathF.Sign(transform.localScale.x))
            if (result.collider.TryGetComponent(out T able))
            {
                if (bodyCo != null && bodyCo.Equals(able)) continue;
                if (minDIS >  xDis&& result.distance<viewDis)
                {
                    minDisRes = able;
                    minDIS = xDis;
                }
            }
        }
        return minDisRes;
    }
}
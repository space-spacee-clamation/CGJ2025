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
        RaycastHit2D[] res = Physics2D.CircleCastAll(eyePoint.position, viewDis, Vector3.right, viewDis);
        T minDisRes = default;
        float minDIS = float.MaxValue;
        T  bodyCo = gameObject.GetComponent<T>();
        foreach (RaycastHit2D result in res)
        {
            if ( Vector3.Dot(((Vector3)result.point - eyePoint.position).normalized, MathF.Sign(transform.localScale.x) * Vector3.right) < MathF.Cos(MathF.Sign(transform.localScale.x) * range / 360 * 2 * MathF.PI))
                continue;
            if (result.collider.TryGetComponent(out T able))
            {
                if (bodyCo != null && bodyCo.Equals(able)) continue;
                if (minDIS >  result.distance)
                {
                    minDisRes = able;
                    minDIS = result.distance;
                }
            }
        }
        return minDisRes;
    }
}
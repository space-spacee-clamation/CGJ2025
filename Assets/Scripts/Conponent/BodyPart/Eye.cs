using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 获得视野前的物体的组件
/// </summary>
public class Eye : MonoBehaviour
{
    [SerializeField] private Transform eyePoint;
    private IControlAble bodyAble;
    [SerializeField] private float viewDis;
    [SerializeField] private LayerMask _mask;

    private void Awake()
    {
        bodyAble=gameObject.GetComponent<IControlAble>();
    }
    public  T GetFacingObjComponent<T>()
    {
        var res = Physics2D.RaycastAll(eyePoint.position, transform.localScale.x  * Vector3.right, viewDis,_mask);
        T minDisRes=default;
        float minDIS=float.MaxValue;
       T  bodyCo= gameObject.GetComponent<T>();
        foreach (var result in res)
        {
           if (result.collider.TryGetComponent(out T able))
           {
               if(bodyCo!=null && bodyCo.Equals(able))continue;
               if (minDIS>result.distance)
               {
                   minDisRes=able;
                   minDIS=result.distance;
               }
           }
        }
        return minDisRes;        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePoint.position, eyePoint.position+transform.localScale.x*viewDis *Vector3.right);
    }
}

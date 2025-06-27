using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 获得视野前的物体的组件
/// </summary>
public class Eye : MonoBehaviour
{
    [SerializeField] private Transform bodyTran;
    private IControlAble bodyAble;
    [SerializeField] private float viewDis;
    [SerializeField] private LayerMask _mask;

    private void Awake()
    {
        bodyAble=bodyTran.GetComponent<IControlAble>();
    }
    public IControlAble GetFacingObjControlAble()
    {
        var res = Physics2D.RaycastAll(transform.position, bodyTran.localScale.x  * Vector3.right, viewDis,_mask);
        IControlAble minDisRes=null;
        float minDIS=float.MaxValue;
        foreach (var result in res)
        {
           if (result.collider.TryGetComponent(out IControlAble able))
           {
               if(bodyAble==able)continue;
               if (minDIS>result.distance)
               {
                   minDisRes=able;
                   minDIS=result.distance;
               }
           }
        }
        Debug.Log("咩咩咩咩");
        return minDisRes;        
    }
    public void SetBodyTrans(Transform body)
    {
        bodyTran = body;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position+bodyTran.localScale.x*viewDis *Vector3.right);
    }
}

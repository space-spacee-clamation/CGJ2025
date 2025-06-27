using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Feet : MonoBehaviour
{
    public Action OnGround;
    public Action OnAir;
    [SerializeField] private float rayLen=2f;
    [SerializeField] private LayerMask layerMask;
    bool isGround;
    public void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLen,layerMask);
        if (hit.collider != null)
        {
            OnGround?.Invoke();
            isGround = true;
        }
        else if(isGround)
        {
            OnAir?.Invoke();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * rayLen);
    }
}

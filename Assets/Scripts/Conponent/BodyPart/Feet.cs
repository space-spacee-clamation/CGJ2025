using System;
using UnityEngine;
public class Feet : MonoBehaviour
{
    [SerializeField] private float rayLen = 2f;
    [SerializeField] private LayerMask layerMask;
    public Action OnAir;
    public Action OnGround;
    public bool IsGround { get; private set; }
    public void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLen, layerMask);
        if ( !IsGround && hit.collider != null)
        {
            OnGround?.Invoke();
            IsGround = true;
        }
        else if (IsGround&& hit.collider == null)
        {
            OnAir?.Invoke();
            IsGround = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * rayLen);
    }
}
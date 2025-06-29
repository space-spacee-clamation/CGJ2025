using UnityEngine;
public class HeadNode : SingleNode
{
    public override void ActiveNode()
    {
        Debug.Log("激活");
        _animator.Play("ShiDrop");
        gameObject.SetActive(true);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimuComponent : ABaseControlAble
{
    protected Queue<SingleNode> m_nodeQueue;
    public override void Input(ControlType type, object param)
    {
        
    }
}
public class HeadNode : SingleNode
{
    
}
public class SingleNode : MonoBehaviour
{
    public GameObject nodeNext;
    public enum FourDir
    {
        UP,Down,Left,Right
    }
    protected SingleNode m_nextNode;
    protected GameObject CreateNode(FourDir dir)
    {
       Vector3 temp=  GetPos( dir);
       GameObject ga= Instantiate(nodeNext, temp, Quaternion.identity);
       ga.transform.SetParent(transform);
       var re = ga.GetComponent<SingleNode>();
       //检测创建是否合法 
       if (re && re.CheckNode())
       {
           re.ActiveNode();
           return ga;
       }
       else
       {
           re.DeletNode();
           return null;
       }
    }
    private void ActiveNode()
    {
        
    }
    private void DeletNode()
    {
        
    }
    private bool CheckNode()
    {
        var res= Physics2D.CircleCastAll(transform.position, 0.45f, Vector3.up);
        foreach (var rayHIt in res)
        {
            if (rayHIt.transform==this)continue;
            return false;
        }
        return true;
    }
    protected Vector3 GetPos(FourDir dir)
    {
        switch (dir)
        {
            case FourDir.UP:
                return transform.position+Vector3.up;
                break;
            case FourDir.Down:
                return transform.position+Vector3.down;
                break;
            case FourDir.Left:
                return transform.position+Vector3.left;
                break;
            case FourDir.Right:
                return transform.position+Vector3.right;
                break;
        }
        return Vector3.zero;
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SlimuComponent : ABaseControlAble
{
    protected Stack<SingleNode> m_nodeQueue;
    [SerializeField] protected SingleNode headNode;
    protected SingleNode nowNode;
    [SerializeField]protected int maxCount=5;
    protected override void Start()
    {
        base.Start();
        nowNode = headNode;
        m_nodeQueue = new Stack<SingleNode>();
        m_nodeQueue.Push(headNode);
    }
    private float creatCD = 0.1f;
    private void Update()
    {
        if (creatCD>0)
        {
            creatCD -= Time.deltaTime;
        }
    }
    public override void Input(ControlType type, object param)
    {
        if(creatCD<0)
        switch (type)
        {
            case ControlType.UP:
                HandelCreate(SingleNode.FourDir.UP);
                break;
            case ControlType.Right:
                HandelCreate(SingleNode.FourDir.Right);
                break;
            case ControlType.Left:
                HandelCreate(SingleNode.FourDir.Left);
                break;
            case ControlType.Down:
                HandelCreate(SingleNode.FourDir.Down);
                break;
        }
    }
    private void HandelCreate(SingleNode.FourDir dir)
    {
        var result= nowNode.CreateNode(dir, out var node);
        switch (result)
        {
            case SingleNode.CreatState.Enter:
                if (maxCount > 0)
                {
                    m_nodeQueue.Push(nowNode);
                    node.ActiveNode();
                    nowNode = node;
                    maxCount--;
                }
                else
                {
                    node.DeletNode();
                }
                break;
            case SingleNode.CreatState.Back:
                if(nowNode.Equals(headNode))break;
                nowNode.DeletNode();
                nowNode = m_nodeQueue.Pop();
                maxCount++;
                break;
            case SingleNode.CreatState.Fail:
                node.DeletNode();
                break;
        }
        creatCD = 0.1f;
    }
}


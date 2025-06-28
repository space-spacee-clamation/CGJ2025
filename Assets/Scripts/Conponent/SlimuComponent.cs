using System.Collections.Generic;
using UnityEngine;
public class SlimuComponent : ABaseControlAble
{
    [SerializeField] protected SingleNode headNode;
    [SerializeField] protected int maxCount = 5;
    private      float   creatCD = 0.3f;
    protected Stack<SingleNode> m_nodeQueue;
    protected SingleNode nowNode;
    protected override void Start()
    {
        base.Start();
        nowNode = headNode;
        m_nodeQueue = new Stack<SingleNode>();
        m_nodeQueue.Push(headNode);
    }
    private void Update()
    {
        if (creatCD >= 0)
        {
            creatCD -= Time.deltaTime;
        }
    }
    public override void Input(ControlType type, object param)
    {
        if (creatCD < 0)
        {
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
        if (type.Equals(ControlType.Fire))
        {
            LeaveControl();
        }
    }
    private void HandelCreate(SingleNode.FourDir dir)
    {
        SingleNode.CreatState result = nowNode.CreateNode(dir, out SingleNode node);
        switch (result)
        {
            case SingleNode.CreatState.Enter:
                if (maxCount > 0)
                {
                    m_nodeQueue.Push(nowNode);
                    node.ActiveNode();
                    nowNode = node;
                    maxCount--;
                    AudioManager.Instance.PlayOnce("BABANoise");
                }
                else
                {
                    node.DeletNode();
                }
                break;
            case SingleNode.CreatState.Back:
                if (nowNode.Equals(headNode)) break;
                nowNode.DeletNode();
                nowNode = m_nodeQueue.Pop();
                maxCount++;
                break;
            case SingleNode.CreatState.Fail:
                node.DeletNode();
                break;
        }
        creatCD = 0.3f;
    }
}
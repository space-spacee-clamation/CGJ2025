using UnityEngine;
using UnityEngine.Serialization;
public class SingleNode : MonoBehaviour
{
    public enum CreatState
    {
        Enter,
        Back,
        Fail,
    }
    public enum FourDir
    {
        UP,
        Down,
        Left,
        Right,
    }
    [FormerlySerializedAs("nextNode")] public GameObject prefabNode;
    public GameObject fatherNode ;
    protected SingleNode m_nextNode;
    [SerializeField] protected Animator _animator;
    public CreatState CreateNode(FourDir dir, out SingleNode node)
    {
        Vector3 temp =  GetPos( dir);
        GameObject ga = Instantiate(prefabNode, temp, GetRot(dir));
        ga.transform.SetParent(transform,true);
        SingleNode re = ga.GetComponent<SingleNode>();
        CreatState reState = CreatState.Fail;
        //检测创建是否合法 
        switch (re.CheckNode())
        {
            case CreatState.Enter:
                m_nextNode = re;
                re.fatherNode = gameObject;
                reState = CreatState.Enter;
                break;
            case CreatState.Back:
                reState = CreatState.Back;
                break;
            case CreatState.Fail:
                reState = CreatState.Fail;
                break;
        }
        node = re;
        return reState;
    }
    public virtual void ActiveNode()
    {
        Debug.Log("激活");
        _animator.Play("ShiGrow");
        gameObject.SetActive(true);
    }
    public void DeletNode()
    {
        //TODO: 删除
        Destroy(gameObject);
    }
    private CreatState CheckNode()
    {
        RaycastHit2D[] res = Physics2D.CircleCastAll(transform.position, 0.75f, Vector3.up);
        foreach (RaycastHit2D rayHIt in res)
        {
            if (rayHIt.transform.Equals(transform)) continue;
            if  (fatherNode != null && rayHIt.transform == fatherNode?.transform) return CreatState.Back;
            return CreatState.Fail;
        }
        return CreatState.Enter;
    }
    protected Quaternion GetRot(FourDir dir)
    {
        switch (dir)
        {
            case FourDir.UP:
                return Quaternion.Euler(0,0,90);
                break;
            case FourDir.Left:
                return Quaternion.Euler(0,0,180);
                break;
            case FourDir.Down:
                return Quaternion.Euler(0, 0, 270);
                break;
                case FourDir.Right:
                    return Quaternion.Euler(0, 0, 0);
                break;
        }
        return Quaternion.identity;
    }
    protected Vector3 GetPos(FourDir dir)
    {
        switch (dir)
        {
            case FourDir.UP:
                return transform.position + Vector3.up*1.5f;
                break;
            case FourDir.Down:
                return transform.position + Vector3.down*1.5f;
                break;
            case FourDir.Left:
                return transform.position + Vector3.left*1.5f;
                break;
            case FourDir.Right:
                return transform.position + Vector3.right*1.5f;
                break;
        }
        return Vector3.zero;
    }
}
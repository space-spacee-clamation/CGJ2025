using Cinemachine;
using UnityEngine;
/// <summary>
///     玩家控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private IControlAble controlAble;
    public bool CouldControl=false;
    [SerializeField] public CinemachineVirtualCamera VirtualCamera;
    private IControlAble ControlAble {
        get
        {
          return   controlAble;
        }
        set {
           controlAble = value;
        }
    }
    private void OnEnable()
    {
        Destroy(Instance);
        Instance = this;
    }
    public void Start()
    {
        ControlAble?.OnControl?.Invoke();
        GameManager.Instance.OnNewLevel += StartController;
    }
    private void StartController()
    {
        CouldControl = true;
    }
    private void Update()
    {
        if(CouldControl)
            InputUpdate();
    }
    public void RemoveControlAble()
    {
        ControlAble?.OnRelease?.Invoke();
        ControlAble = GameManager.Instance.GetPlayer();
        ControlAble?.OnControl?.Invoke();
    }
    public void SetControlAble(IControlAble controlAble)
    {
        if(controlAble==null) return;
        if (controlAble == ControlAble || !controlAble.ControllAble()) return;
        if (controlAble != null) ControlAble?.OnRelease?.Invoke();
        ControlAble = controlAble;
        ControlAble?.OnControl?.Invoke();
    }
    private bool useTime = false;
    
    public void InputUpdate()
    {
        //默认值
        if (ControlAble == null) ControlAble = GameManager.Instance.GetPlayer();

        float tempHorizontal = Input.GetAxis("Horizontal");
        if (tempHorizontal != 0)
        {
            ControlAble.Input(tempHorizontal > 0 ? ControlType.Right : ControlType.Left, tempHorizontal);
        }
        float tempVertical = Input.GetAxis("Vertical");
        if (tempVertical != 0)
        {
            ControlAble.Input(tempVertical > 0 ? ControlType.UP : ControlType.Down, tempVertical);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ControlAble.Input(ControlType.Jump, null);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ControlAble.Input(ControlType.Fire, null);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ControlAble.Input(ControlType.Next, null);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.ReStart();
        }

        //TODO: 其它交互
    }
    public void SetControllerActive(bool co)
    {
        CouldControl = co;
    }
}
using UnityEngine;
/// <summary>
///     玩家控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private IControlAble controlAble;
    public bool CouldControl=false;
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
    private float fireCD = 0.1f;
    public void SetControlAble(IControlAble controlAble)
    {
        if (controlAble == ControlAble) return;
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
            useTime = true;
        }
        float tempVertical = Input.GetAxis("Vertical");
        if (tempVertical != 0)
        {
            ControlAble.Input(tempVertical > 0 ? ControlType.UP : ControlType.Down, tempVertical);
            useTime = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ControlAble.Input(ControlType.Jump, null);
            useTime = true;
        }
        if (Input.GetKeyDown(KeyCode.F) && fireCD<0)
        {
            ControlAble.Input(ControlType.Fire, null);
            fireCD = 0.1f;
            useTime = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ControlAble.Input(ControlType.Next, null);
            useTime = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.ReStart();
        }
        if (useTime)
        {
            TimeController.Instance.ChangeTick(Time.deltaTime);
            useTime = false;
        }
        if (fireCD>=0)
        {
            fireCD -= Time.deltaTime;
        }
        //TODO: 其它交互
    }
    public void SetControllerActive(bool co)
    {
        CouldControl = co;
    }
}
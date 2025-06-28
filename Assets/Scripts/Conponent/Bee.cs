using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bee : ABaseControlAble , IChangeWithTime
{

    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float MaxDis;
    private PowerUseAbleComponent powerUseAbleComponent;
    protected float dis;
    protected Rigidbody2D rigidbody2D;
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    protected override void Start()
    {
        base.Start();
        TimeController.Instance.SubObj(this);
        powerUseAbleComponent.AddCallBack(PowerEnum.Fire, GetFire);
    }
    private void GetFire()
    {
        isFired = true;
    }
    bool isFired;
    public override bool ControllAble()
    {
        return isFired;
    }
    protected void FixedUpdate()
    {
        dis += maxSpeed * Time.fixedDeltaTime;
        if (dis >= MaxDis)
        {
            
        }
    }
    public void ChangeWithWeather(GameTimeEnum time)
    {
        switch (time)
        {
            case GameTimeEnum.Day:
                gameObject.SetActive(false);
                break;
            case GameTimeEnum.Night:
                gameObject.SetActive(true);
                break;
        }
    }
    protected  void HorizontalMove(float value)
    {
        AudioManager.Instance.PlayOnce("Fly");
        rigidbody2D.velocity = new Vector2(value * maxSpeed, rigidbody2D.velocity.y);
    }
    protected  void VerticalMove(float value)
    {
        AudioManager.Instance.PlayOnce("Fly");
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, value * maxSpeed);
    }
    public override void Input(ControlType type, object param)
    {
        switch (type)
        {
            case ControlType.UP:
                VerticalMove((float)param);
                break;
            case ControlType.Right:
                HorizontalMove((float)param);
                break;
            case ControlType.Left:
                HorizontalMove((float)param);
                break;
            case ControlType.Down:
                VerticalMove((float)param);
                break;
            case ControlType.Fire:
                Fire();
                break;
        }
    }
    private void Fire()
    {
        LeaveControl();
    }
}
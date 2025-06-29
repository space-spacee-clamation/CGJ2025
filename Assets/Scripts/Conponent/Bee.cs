using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bee : ABaseControlAble , IChangeWithTime
{

    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float MaxDis;
   [SerializeField]  protected float dis;
    private bool isFired;
   [SerializeField]  private PowerUseAbleComponent powerUseAbleComponent;
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
    protected void FixedUpdate()
    {
        dis += rigidbody2D.velocity.magnitude * Time.fixedDeltaTime;
        if (dis >= MaxDis)
        {
            Finish();
        }
    }
    private void Finish()
    {
        LeaveControl();
        GameManager.Instance.PlayerTran.position = transform.position;
        Destroy(gameObject);
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
    private void GetFire()
    {
        isFired = true;
    }
    public override bool ControllAble()
    {
        return isFired;
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
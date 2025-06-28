
using System;
using ControlAble;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bee : ABaseControlAble , IChangeWithTime
{
    protected Rigidbody2D rigidbody2D;
    [SerializeField] protected float maxSpeed;
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    protected  void HorizontalMove(float value)
    {
        rigidbody2D.velocity = new Vector2(value * maxSpeed, rigidbody2D.velocity.y);
    }
    protected  void VerticalMove(float value)
    {
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, value * maxSpeed);
    }
    protected override void Start()
    {
        base.Start();
        TimeController.Instance.SubObj(this);
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
}

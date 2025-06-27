using System;
using UnityEngine;
namespace ControlAble
{
    /// <summary>
    /// 使用正常的移动逻辑，包括跳跃和简单移动
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class ANormalMove : ABaseControlAble 
    {
        [SerializeField] protected Feet _feet;
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected float jumpForce;
        [SerializeField] protected bool isGround;
        protected Rigidbody2D rigidbody2D;
        /// <summary>
        ///     防止连续按跳跃
        /// </summary>
        protected float tickTime = 0.5f;
        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            _feet = GetComponentInChildren<Feet>();
        }
        private void Start()
        {
            //TODO : 插音效
            _feet.OnGround += () => {
                isGround = true;
            };
            OnStart();
            OnRelease += Release;
            OnControl += Control;
        }
        private void Control()
        {
            rigidbody2D.mass = 1;
        }
        private void Release()
        {
            //暂时先这样吧😋
            rigidbody2D.mass = 1000000;
        }
        private void Update()
        {
            if (tickTime > 0)
            {
                tickTime -= Time.deltaTime;
            }
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
                case ControlType.Jump:
                    Jump();
                    break;
            }
        }
        protected virtual void Jump()
        {
            if (isGround && tickTime <= 0)
            {
                rigidbody2D.AddForce(new Vector2(0,  jumpForce * rigidbody2D.mass), ForceMode2D.Impulse);
                tickTime = 0.5f;
            }
        }
        protected virtual void HorizontalMove(float  value)
        {
            rigidbody2D.velocity = new Vector2(value * maxSpeed, rigidbody2D.velocity.y);
            transform.localScale = new Vector3((MathF.Abs(value)>0.05? Mathf.Sign( value):transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        protected virtual void VerticalMove(float  value)
        {
            //没有喵
        }
    }
}
﻿using System;
using UnityEngine;
namespace ControlAble
{
    /// <summary>
    ///     使用正常的移动逻辑，包括跳跃和简单移动
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class ANormalMove : ABaseControlAble
    {
        [SerializeField] protected Feet _feet;
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected float jumpDis;
        protected Rigidbody2D rigidbody2D;
        /// <summary>
        ///     防止连续按跳跃
        /// </summary>
        protected float tickTime = 0.5f;
        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }
        protected override void Start()
        {
            base.Start();
            //TODO : 插音效
            OnStart();
            OnRelease += Release;
            OnControl += Control;
            OnRelease += () => {
                gameObject.tag = "Untagged";
            };
            OnControl += () => {
                gameObject.tag = "Player";
            };
            rigidbody2D = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            if (tickTime > 0)
            {
                tickTime -= Time.deltaTime;
            }
            OnUpdate();
        }
        protected virtual void OnUpdate()
        {

        }
        protected virtual void OnStart()
        {
        }
        protected virtual void Control()
        {
        }
        protected virtual void Release()
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
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
                case ControlType.Fire:
                    Fire();
                    break;
                default:
                    GetOtherInput(type, param);
                    break;
            }

        }
        protected virtual void Fire()
        {
            LeaveControl();
        }
        protected virtual void GetOtherInput(ControlType type, object o)
        {

        }
        protected virtual void Jump()
        {
            if (_feet.IsGround && tickTime <= 0)
            {
                float gravity = Physics2D.gravity.y;
                float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpDis);
                rigidbody2D.AddForce(new Vector2(0,  jumpVelocity), ForceMode2D.Impulse);
                tickTime = 0.5f;
                OnJump();
            }
        }
        protected virtual void OnJump()
        {
           
        }
        protected virtual void HorizontalMove(float  value)
        {
            rigidbody2D.velocity = new Vector2(value * maxSpeed, rigidbody2D.velocity.y);
            transform.localScale = new Vector3(MathF.Abs(value) > 0.05 ? Mathf.Sign( value) : transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        protected virtual void VerticalMove(float  value)
        {
            //没有喵
        }
    }
}
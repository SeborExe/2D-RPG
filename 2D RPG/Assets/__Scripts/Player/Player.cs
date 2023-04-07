using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }

    [field: Header("Move Info")]
    [field:SerializeField] public float MoveSpeed { get; private set; } = 8f;
    [field:SerializeField] public float JumpForce { get; private set; } = 12f;

    [field: Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    [field: Header("Direction Info")]
    public bool FacingRight { get; private set; } = true;
    public int FacingDir { get; private set; } = 1;

    [field: Header("Dash Info")]
    [field: SerializeField] public float DashSpeed { get; private set; } = 25f;
    [field: SerializeField] public float DashDuration { get; private set; } = 0.3f;
    [field: SerializeField] public float DashCooldown { get; private set; } = 0.3f;
    public float DashDir { get; private set; }
    private float dashTimer;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(StateMachine, this, Resources.Idle);
        MoveState = new PlayerMoveState(StateMachine, this, Resources.Move);
        JumpState = new PlayerJumpState(StateMachine, this, Resources.Jump);
        AirState = new PlayerAirState(StateMachine, this, Resources.Jump);
        DashState = new PlayerDashState(StateMachine, this, Resources.Dash);

        Animator = GetComponentInChildren<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();

        CheckForDash();
        UpdateTimers();
    }

    public void SetVelocity(float xVelocity, float yVelicoty)
    {
        Rigidbody2D.velocity = new Vector2(xVelocity, yVelicoty);
        FlipController(xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public void Flip()
    {
        FacingRight = !FacingRight;
        FacingDir *= -1;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void FlipController(float move)
    {
        if (move > 0 && !FacingRight)
            Flip();
        else if (move < 0 && FacingRight)
            Flip();
    }

    private void CheckForDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0f)
        {
            DashDir = Input.GetAxisRaw("Horizontal");

            if (DashDir == 0)
            {
                DashDir = FacingDir;
            }

            dashTimer = DashCooldown;
            StateMachine.ChangeState(DashState);
        }
    }

    private void UpdateTimers()
    {
        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0f) { dashTimer = 0f; }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}

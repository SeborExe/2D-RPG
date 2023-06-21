using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Player : Entity
{
    public event Action OnDashUsed;
    public event Action OnParryUsed;
    public event Action OnCrystalUsed;
    public event Action OnSwordUsed;
    public event Action OnBlackholeUsed;
    public event Action OnFlaskUsed;

    public PlayerStateMachine StateMachine { get; private set; }
    public SkillManager SkillManager { get; private set; }
    public PlayerFX PlayerFX { get; private set; }
    public GameObject Sword { get; private set; }

    #region States
    [field: Header("States")]
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttack { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    public PlayerBlackholeState BlackholeState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    #endregion

    #region Move Info
    [field: Header("Move Info")]
    [field:SerializeField] public float MoveSpeed { get; private set; } = 8f;
    [field:SerializeField] public float JumpForce { get; private set; } = 12f;
    [field:SerializeField] public float SwordReturnImpact { get; private set; }
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    #endregion

    #region Dash Info
    [field: Header("Dash Info")]
    [field: SerializeField] public float DashSpeed { get; private set; } = 25f;
    [field: SerializeField] public float DashDuration { get; private set; } = 0.3f;
    public float DashDir { get; private set; }
    private float defaultDashSpeed;
    #endregion

    #region In Air Slowdonw
    [field: Header("In Air slowndown")]
    [field: SerializeField] public float WallJumpMovementSlowdown { get; private set; } = 0.4f;
    [field: SerializeField] public float InAirMovementSlowdown { get; private set; } = 0.75f;
    [field: SerializeField] public float JumpMovementSlowdown { get; private set; } = 0.6f;
    #endregion

    #region AttackDetails
    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    #endregion

    public bool isBusy { get; private set; }

    protected override void Awake()
    {
        StateMachine = new PlayerStateMachine();
        PlayerFX = GetComponent<PlayerFX>();

        IdleState = new PlayerIdleState(StateMachine, this, Resources.Idle);
        MoveState = new PlayerMoveState(StateMachine, this, Resources.Move);
        JumpState = new PlayerJumpState(StateMachine, this, Resources.Jump);
        AirState = new PlayerAirState(StateMachine, this, Resources.Jump);
        DashState = new PlayerDashState(StateMachine, this, Resources.Dash);
        WallSlideState = new PlayerWallSlideState(StateMachine, this, Resources.WallSlide);
        WallJumpState = new PlayerWallJumpState(StateMachine, this, Resources.Jump);
        CounterAttackState = new PlayerCounterAttackState(StateMachine, this, Resources.CounterAttack);
        BlackholeState = new PlayerBlackholeState(StateMachine, this, Resources.Jump);

        AimSwordState = new PlayerAimSwordState(StateMachine, this, Resources.AimSword);
        CatchSwordState = new PlayerCatchSwordState(StateMachine, this, Resources.CatchSword);

        PrimaryAttack = new PlayerPrimaryAttackState(StateMachine, this, Resources.Attack);
        DeadState = new PlayerDeadState(StateMachine, this, Resources.Die);

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        SkillManager = SkillManager.Instance;

        StateMachine.Initialize(IdleState);

        defaultMoveSpeed = MoveSpeed;
        defaultJumpForce = JumpForce;
        defaultDashSpeed = DashSpeed;
    }

    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();
        StateMachine.CurrentState.Update();

        CheckForCrystal();
        CheckForDash();
        CheckForFlash();
    }

    public override void SlowEntity(float slowPercentage, float slowDuration)
    {
        MoveSpeed = MoveSpeed * (1 - slowPercentage);
        JumpForce = JumpForce * (1 - slowPercentage);
        DashSpeed = DashSpeed * (1 - slowPercentage);
        Animator.speed = Animator.speed * (1 - slowPercentage);

        Invoke(nameof(ReturnDefaultSpeed), slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        MoveSpeed = defaultMoveSpeed;
        JumpForce = defaultJumpForce;
        DashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject newSword)
    {
        Sword = newSword;
    }

    public void CatchSword()
    {
        StateMachine.ChangeState(CatchSwordState);
        Destroy(Sword);
    }

    private void CheckForDash()
    {
        if (IsWallDetected())
            return;

        if (!SkillManager.DashSkill.dashUnlocked)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.DashSkill.CanUseSkill())
        {
            DashDir = Input.GetAxisRaw("Horizontal");

            if (DashDir == 0)
                DashDir = FacingDir;

            StateMachine.ChangeState(DashState);
            OnDashUsed?.Invoke();
        }
    }

    private void CheckForCrystal()
    {
        if (Input.GetKeyDown(KeyCode.F) && SkillManager.CrystalSkill.crystalUnloced)
        {
            SkillManager.CrystalSkill.CanUseSkill();
            OnCrystalUsed?.Invoke();
        }
    }

    private void CheckForFlash()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Instance.UseFlask();
            OnFlaskUsed?.Invoke();
        }
    }

    public async Task BusyFor(int miliseconds)
    {
        isBusy = true;

        await Task.Delay(miliseconds);

        isBusy = false;
    }

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(DeadState);
    }

    protected override void SetUpZeroKnockbackPower()
    {
        knockbackPower = Vector2.zero;
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public void InvokeOnParryUsed() => OnParryUsed?.Invoke();
    public void InvokeOnSwordUsed() => OnSwordUsed?.Invoke();
    public void InvokeOnBlackholeUsed() => OnBlackholeUsed?.Invoke();
}

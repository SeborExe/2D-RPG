using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : Enemy
{
    #region States
    public ArcherIdleState IdleState { get; private set; }
    public ArcherMoveState MoveState { get; private set; }
    public ArcherBattleState BattleState { get; private set; }
    public ArcherAttackState AttackState { get; private set; }
    public ArcherStunnedState StunnedState { get; private set; }
    public ArcherDeadState DeadState { get; private set; }
    public ArcherJumpState JumpState { get; private set; }
    #endregion

    [field: Header("Archer Info")]
    [field: SerializeField] public Vector2 JumpVelocity { get; private set; }
    [field: SerializeField] public GameObject ArrowPrefab { get; private set; }
    [field: SerializeField] public float SaveDistance { get; private set; }
    [field: SerializeField] public float JumpCooldown { get; private set; }
    [field: SerializeField] public float ArrowSpeed { get; private set; }
    [HideInInspector] public float lastTimeJumped;

    [Header("Additional Checks")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new ArcherIdleState(StateMachine, this, Resources.Idle, this);
        MoveState = new ArcherMoveState(StateMachine, this, Resources.Move, this);
        BattleState = new ArcherBattleState(StateMachine, this, Resources.Idle, this);
        AttackState = new ArcherAttackState(StateMachine, this, Resources.Attack, this);
        StunnedState = new ArcherStunnedState(StateMachine, this, Resources.Stun, this);
        JumpState = new ArcherJumpState(StateMachine, this, Resources.Jump, this);

        if (HasDeadAnimation)
            DeadState = new ArcherDeadState(StateMachine, this, Resources.Die, this);
        else
            DeadState = new ArcherDeadState(StateMachine, this, LastAnimBoolName, this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            StateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(DeadState);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(ArrowPrefab, attackCheck.position, Quaternion.identity);
        newArrow.GetComponent<ArrowController>().SetupArrow(ArrowSpeed * FacingDir, CharacterStats);
    }
    public bool GroundBehindCheck() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, whatIsGround);

    public bool IsWallBehind() => Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDir, wallCheckDistance + 3, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
    }
}

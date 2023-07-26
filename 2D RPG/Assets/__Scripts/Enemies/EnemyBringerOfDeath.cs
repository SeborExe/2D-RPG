using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBringerOfDeath : Enemy
{
    #region States
    public BringerOfDeathIdleState IdleState { get; private set; }
    public BringerOfDeathMoveState MoveState { get; private set; }
    public BringerOfDeathBattleState BattleState { get; private set; }
    public BringerOfDeathAttackState AttackState { get; private set; }
    public BringerOfDeathTeleportState TeleportState { get; private set; }
    public BringerOfDeathSpellCastState SpellCastState { get; private set; }
    public BringerOfDeathDeadState DeadState { get; private set; }
    #endregion

    [Header("Teleport details")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;

    private CapsuleCollider2D collider;

    protected override void Awake()
    {
        base.Awake();
        collider = GetComponent<CapsuleCollider2D>();

        IdleState = new BringerOfDeathIdleState(StateMachine, this, Resources.Idle, this);
        MoveState = new BringerOfDeathMoveState(StateMachine, this, Resources.Move, this);
        BattleState = new BringerOfDeathBattleState(StateMachine, this, Resources.Move, this);
        AttackState = new BringerOfDeathAttackState(StateMachine, this, Resources.Attack, this);
        TeleportState = new BringerOfDeathTeleportState(StateMachine, this, Resources.Teleport, this);
        SpellCastState = new BringerOfDeathSpellCastState(StateMachine, this, Resources.Cast, this);

        if (HasDeadAnimation)
            DeadState = new BringerOfDeathDeadState(StateMachine, this, Resources.Die, this);
        else
            DeadState = new BringerOfDeathDeadState(StateMachine, this, LastAnimBoolName, this);
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

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(DeadState);
    }

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (collider.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            Debug.Log("Looking for new position");
            FindPosition();
        }
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);

    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }
}

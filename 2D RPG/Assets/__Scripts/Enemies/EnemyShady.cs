using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShady : Enemy
{
    #region States
    public ShadyIdleState IdleState { get; private set; }
    public ShadyMoveState MoveState { get; private set; }
    public ShadyBattleState BattleState { get; private set; }
    //public SkeletonAttackState AttackState { get; private set; }
    public ShadyStunnedState StunnedState { get; private set; }
    public ShadyDeadState DeadState { get; private set; }
    #endregion

    [field: Header("Shady Info")]
    [field: SerializeField] public float RunSpeed { get; private set; }
    [SerializeField] public GameObject explosionPrefab;
    [SerializeField] public float growSpeed;
    [SerializeField] public float maxSize;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new ShadyIdleState(StateMachine, this, Resources.Idle, this);
        MoveState = new ShadyMoveState(StateMachine, this, Resources.Move, this);
        BattleState = new ShadyBattleState(StateMachine, this, Resources.Run, this);
        StunnedState = new ShadyStunnedState(StateMachine, this, Resources.Stun, this);
        DeadState = new ShadyDeadState(StateMachine, this, Resources.Explode, this);
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
        GameObject newExplosion = Instantiate(explosionPrefab, attackCheck.position, Quaternion.identity);
        newExplosion.GetComponent<ShadyExplosion>().SetupExplosion(CharacterStats, growSpeed, maxSize, attackCheckRadius);

        CapsuleCollider.enabled = false;
        Rigidbody2D.gravityScale = 0;
    }

    public void SelfDestroy() => Destroy(gameObject);
}

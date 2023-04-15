using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region States
    public SkeletonIdleState IdleState { get; private set; }
    public SkeletonMoveState MoveState { get; private set; }
    public SkeletonBattleState BattleState { get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        IdleState = new SkeletonIdleState(StateMachine, this, Resources.Idle, this);
        MoveState = new SkeletonMoveState(StateMachine, this, Resources.Move, this);
        BattleState = new SkeletonBattleState(StateMachine, this, Resources.Move, this);
        AttackState = new SkeletonAttackState(StateMachine, this, Resources.Attack, this);
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
}

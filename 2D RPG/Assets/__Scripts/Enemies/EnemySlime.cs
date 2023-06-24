using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : Enemy
{
    #region States
    public SlimeIdleState IdleState { get; private set; }
    public SlimeMoveState MoveState { get; private set; }
    public SlimeBattleState BattleState { get; private set; }
    public SlimeAttackState AttackState { get; private set; }
    public SlimeStunnedState StunnedState { get; private set; }
    public SlimeDeadState DeadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        IdleState = new SlimeIdleState(StateMachine, this, Resources.Idle, this);
        MoveState = new SlimeMoveState(StateMachine, this, Resources.Move, this);
        BattleState = new SlimeBattleState(StateMachine, this, Resources.Move, this);
        AttackState = new SlimeAttackState(StateMachine, this, Resources.Attack, this);
        StunnedState = new SlimeStunnedState(StateMachine, this, Resources.Stun, this);

        if (HasDeadAnimation)
            DeadState = new SlimeDeadState(StateMachine, this, Resources.Die, this);
        else
            DeadState = new SlimeDeadState(StateMachine, this, LastAnimBoolName, this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
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
}

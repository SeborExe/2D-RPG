using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : EnemyState
{
    EnemySkeleton enemy;

    public SkeletonIdleState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemySkeleton enemy) : base(stateMachine, enemy, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.IdleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0f)
            stateMachine.ChangeState(enemy.MoveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

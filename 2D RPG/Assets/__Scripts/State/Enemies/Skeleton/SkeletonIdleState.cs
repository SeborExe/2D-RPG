using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemySkeleton enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
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

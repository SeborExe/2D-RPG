using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private EnemySkeleton enemy;

    public SkeletonAttackState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemySkeleton enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(0, 0);

        if (triggerCalled)
            stateMachine.ChangeState(enemy.BattleState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttack = Time.time;
    }
}

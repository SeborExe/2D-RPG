using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : EnemyState
{
    private EnemyArcher enemy;

    public ArcherAttackState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyArcher enemy) : base(stateMachine, enemyBase, animBoolName)
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

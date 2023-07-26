using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathTeleportState : EnemyState
{
    private EnemyBringerOfDeath enemy;

    public BringerOfDeathTeleportState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyBringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.FindPosition();
        stateTimer = 1f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0f)
            stateMachine.ChangeState(enemy.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

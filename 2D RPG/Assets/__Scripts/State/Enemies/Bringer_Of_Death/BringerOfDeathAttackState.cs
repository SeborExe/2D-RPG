using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathAttackState : EnemyState
{
    private EnemyBringerOfDeath enemy;
    public BringerOfDeathAttackState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyBringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.chanceToTeleport += 5;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(0, 0);

        if (triggerCalled)
        {
            if (enemy.CanTeleport())
                stateMachine.ChangeState(enemy.TeleportState);
            else
                stateMachine.ChangeState(enemy.BattleState);
        }    
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttack = Time.time;
    }
}

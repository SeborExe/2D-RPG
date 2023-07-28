using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathIdleState : EnemyState
{
    private EnemyBringerOfDeath enemy;

    public BringerOfDeathIdleState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyBringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
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

        if (Vector2.Distance(PlayerManager.Instance.player.transform.position, enemy.transform.position) < 7)
            enemy.bossFightBegun = true;

        if (stateTimer <= 0 && enemy.bossFightBegun)
            stateMachine.ChangeState(enemy.BattleState);
    }

    public override void Exit()
    {
        base.Exit();

        //AudioManager.Instance.PlaySFX(24, enemy.transform);
    }
}

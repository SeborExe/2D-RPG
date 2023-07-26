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
        
    }

    public override void Exit()
    {
        base.Exit();

        //AudioManager.Instance.PlaySFX(24, enemy.transform);
    }
}

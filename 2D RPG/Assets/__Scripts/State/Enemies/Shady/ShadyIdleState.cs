using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyIdleState : ShadyGroundedState
{
    public ShadyIdleState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyShady enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
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

        //AudioManager.Instance.PlaySFX(24, enemy.transform);
    }
}

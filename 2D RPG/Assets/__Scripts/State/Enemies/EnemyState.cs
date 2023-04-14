using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    protected bool triggerCalled;
    protected float stateTimer;

    private int animBoolName;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemy, int animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.Animator.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemy.Animator.SetBool(animBoolName, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;

    protected bool triggerCalled;
    protected float stateTimer;

    private int animBoolName;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemyBase = enemyBase;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.Animator.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        UpdateTimers();
    }

    public virtual void Exit()
    {
        enemyBase.Animator.SetBool(animBoolName, false);
    }

    private void UpdateTimers()
    {
        if (stateTimer > 0f)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer < 0f) 
                stateTimer = 0f;
        }
    }

    public virtual void AnimationFinishTrigger() => triggerCalled = true;
}

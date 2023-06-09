using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    private int animBoolName;

    protected float xInput;
    protected float yInput;
    protected float dashTimer = 0f;
    protected float stateTimer = 0f;

    protected bool triggerCalled;

    public PlayerState(PlayerStateMachine stateMachine, Player player, int animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.Animator.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.Animator.SetFloat(Resources.yVelocity, player.Rigidbody2D.velocity.y);

        UpdateTimers();
    }

    public virtual void Exit()
    {
        player.Animator.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    private void UpdateTimers()
    {
        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0f) { dashTimer = 0f; }
        }

        if (stateTimer > 0f)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer < 0f) { stateTimer = 0f; }
        }
    }
}

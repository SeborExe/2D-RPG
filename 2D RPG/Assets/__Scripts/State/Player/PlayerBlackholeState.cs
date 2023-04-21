using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed;
    private float defaultGravity;

    public PlayerBlackholeState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = player.Rigidbody2D.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        player.Rigidbody2D.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            player.Rigidbody2D.velocity = new Vector2(0, 15);
        }

        if (stateTimer <= 0)
        {
            player.Rigidbody2D.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                if (player.SkillManager.BlackholeSkill.CanUseSkill()) 
                    skillUsed = true;
            }
        }

        if (player.SkillManager.BlackholeSkill.SkillCompleted())
            stateMachine.ChangeState(player.AirState);
    }

    public override void Exit()
    {
        base.Exit();

        player.Rigidbody2D.gravityScale = defaultGravity;
        player.MakeTransparent(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();


        if (Input.GetKeyDown(KeyCode.R) && player.SkillManager.BlackholeSkill.blackHoleUnlocked)
            stateMachine.ChangeState(player.BlackholeState);

        if (Input.GetKeyDown(KeyCode.Q) && HasNoSword() && player.SkillManager.SwordSkill.swordUnlocked)
            stateMachine.ChangeState(player.AimSwordState);

        if (Input.GetKeyDown(KeyCode.Mouse1) && player.SkillManager.ParrySkill.parryUnlocked)
            stateMachine.ChangeState(player.CounterAttackState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.PrimaryAttack);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.AirState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.JumpState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool HasNoSword()
    {
        if (!player.Sword)
            return true;

        player.Sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}

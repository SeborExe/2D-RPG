using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    private int busyAfterThrowSwordInMiliseconds = 200;

    public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SkillManager.SwordSkill.DotsActive(true);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, 0);

        if (Input.GetKeyUp(KeyCode.Q))
            stateMachine.ChangeState(player.IdleState);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePosition.x && player.FacingDir == 1)
            player.Flip();
        else if (player.transform.position.x < mousePosition.x && player.FacingDir == -1)
            player.Flip();
    }

    public async override void Exit()
    {
        base.Exit();

        await player.BusyFor(busyAfterThrowSwordInMiliseconds);
    }
}

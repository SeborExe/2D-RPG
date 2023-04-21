using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;

    public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.Animator.SetBool(Resources.SuccessfulCounterAttack, false);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, 0);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                if (enemy.CanBeStunned())
                {
                    stateTimer = Mathf.Infinity; //any value bigger than 1
                    player.Animator.SetBool(Resources.SuccessfulCounterAttack, true);

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.SkillManager.CloneSkill.CreateCloneOnCounterAttack(enemy.transform);
                    }
                }
            }
        }

        if (stateTimer <= 0 || triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

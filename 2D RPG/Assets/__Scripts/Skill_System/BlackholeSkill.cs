using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [Space]

    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    [Space]
    [SerializeField] private int attacksAmount;
    [SerializeField] private float cloneAttackCooldown;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab);
        BlackholeSkillController blackholeController = newBlackhole.GetComponent<BlackholeSkillController>();
        blackholeController.SetUpBlackhole(maxSize, growSpeed, shrinkSpeed, attacksAmount, cloneAttackCooldown);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}

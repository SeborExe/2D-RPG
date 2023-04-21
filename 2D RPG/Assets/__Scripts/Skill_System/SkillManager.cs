using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SingletonMonobehaviour<SkillManager>
{
    public DashSkill DashSkill { get; private set; }
    public CloneSkill CloneSkill { get; private set; }
    public SwordSkill SwordSkill { get; private set; }
    public BlackholeSkill BlackholeSkill { get; private set; }
    public CrystalSkill CrystalSkill { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        DashSkill = GetComponent<DashSkill>();
        CloneSkill = GetComponent<CloneSkill>();
        SwordSkill = GetComponent<SwordSkill>();
        BlackholeSkill = GetComponent<BlackholeSkill>();
        CrystalSkill = GetComponent<CrystalSkill>();
    }
}

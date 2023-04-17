using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SingletonMonobehaviour<SkillManager>
{
    public DashSkill DashSkill { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        DashSkill = GetComponent<DashSkill>();
    }
}

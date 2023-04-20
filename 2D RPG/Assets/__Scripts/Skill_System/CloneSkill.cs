using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private CloneSkillController clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float colorLoosingSpeed;
    [Space, SerializeField] private bool canAttack;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        CloneSkillController newClone = Instantiate(clonePrefab);
        newClone.SetupClone(clonePosition, cloneDuration, colorLoosingSpeed, canAttack, offset);
    }
}

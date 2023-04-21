using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private CloneSkillController clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float colorLoosingSpeed;
    [Space]
    [SerializeField] private bool canAttack;
    [Space, Header("Create clone On:")]
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashEnd;
    [SerializeField] private bool canCreateCloneOnCounterAttack;
    [Space, Header("Clone duplication")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    [Space, Header("Crystal instead of clone")]
    [SerializeField] private bool crystalInsteadOfClone;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.Instance.CrystalSkill.CreateCrystal();
            return;
        }

        CloneSkillController newClone = Instantiate(clonePrefab);
        newClone.SetupClone(clonePosition, cloneDuration, colorLoosingSpeed, canAttack, offset, FindClosestEnemy(newClone.transform), canDuplicateClone, 
            chanceToDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashEnd()
    {
        if (createCloneOnDashEnd)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform, float offsetToEnemy = 1.5f)
    {
        if (canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(enemyTransform, new Vector3(offsetToEnemy * player.FacingDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform enemyTransform, Vector3 offset, float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        CreateClone(enemyTransform.transform, offset);
    }

    public bool GetCrystalInsteadOfClone() => crystalInsteadOfClone;
}

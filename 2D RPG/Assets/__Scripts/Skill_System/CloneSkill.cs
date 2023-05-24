using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private CloneSkillController clonePrefab;
    [SerializeField] private float attackMultiplier;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float colorLoosingSpeed;
    [Space]
    [Header("Clone Attack")]
    [SerializeField] private SkillTreeSlotUI cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggresive Clone")]
    [SerializeField] private SkillTreeSlotUI aggresiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Space, Header("Clone duplication")]
    [SerializeField] private SkillTreeSlotUI multipleCloneUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    [Space, Header("Crystal instead of clone")]
    [SerializeField] private SkillTreeSlotUI crystalInsteadCloneUnlockButton;
    [SerializeField] private bool crystalInsteadOfClone;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadClone);
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
            chanceToDuplicate, player, attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform enemyTransform, float offsetToEnemy = 1.5f)
    {
        StartCoroutine(CloneDelayCoroutine(enemyTransform, new Vector3(offsetToEnemy * player.FacingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform enemyTransform, Vector3 offset, float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        CreateClone(enemyTransform.transform, offset);
    }

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggresiveCloneAttackMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (multipleCloneUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInsteadClone()
    {
        if (crystalInsteadCloneUnlockButton.unlocked)
            crystalInsteadOfClone = true;
    }

    public bool GetCrystalInsteadOfClone() => crystalInsteadOfClone;
}

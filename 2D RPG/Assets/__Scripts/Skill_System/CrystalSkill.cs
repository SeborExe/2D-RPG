using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;

    [Header("Crystal")]
    [SerializeField] private SkillTreeSlotUI unlockCrystalButton;
    public bool crystalUnloced { get; private set; }

    [Header("Crystal Mirage")]
    [SerializeField] private SkillTreeSlotUI unlockCrystalMirageButton;
    [SerializeField] private bool cloneInsteadCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private SkillTreeSlotUI unlockExplosiveCrystalButton;
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private SkillTreeSlotUI unlockMovingCrystalButton;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canMove;

    [Header("Multi Crystal")]
    [SerializeField] private SkillTreeSlotUI unlockMultiCrystalButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int crystalAmount;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    private GameObject currentCrystal;

    protected override void Start()
    {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCrystalMirageButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultiCrystal);
    }

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMultiCrystal();
    }

    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnloced = true;
    }

    private void UnlockCrystalMirage()
    {
        if (unlockCrystalMirageButton.unlocked)
            cloneInsteadCrystal = true;
    }

    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveCrystalButton.unlocked)
            canExplode = true;
    }

    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMove = true;
    }

    private void UnlockMultiCrystal()
    {
        if (unlockMultiCrystalButton.unlocked)
            canUseMultiStacks = true;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal()) return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMove) return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadCrystal)
            {
                SkillManager.Instance.CloneSkill.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<CrystalSkillController>().FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController crystal = currentCrystal.GetComponent<CrystalSkillController>();
        crystal.SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    public void CurrentCrystalChooseRandonTarget(float radius) => currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy(radius);

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == crystalAmount)
                    Invoke(nameof(ResetAbility), useTimeWindow);

                SetCooldown(0);

                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(
                    crystalDuration, canExplode, canMove, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0)
                {
                    SetCooldown(multiStackCooldown);
                    RefilCrystal();
                }

                return true;
            }
        }

        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = crystalAmount - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0) return;

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private SkillTreeSlotUI parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Blodsucker")]
    [SerializeField] private SkillTreeSlotUI blodsuckerUnlockButton;
    [SerializeField, Range(0f, 1f)] private float restoreHealthAmount;
    public bool blodsuckerUnlocked { get; private set; }

    [Header("Mirrage Attack")]
    [SerializeField] private SkillTreeSlotUI mirrageAttackButton;
    public bool mirrageAttackUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (blodsuckerUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.CharacterStats.GetMaxHealthValue() * restoreHealthAmount);
            player.CharacterStats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        blodsuckerUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBloodsucker);
        mirrageAttackButton.GetComponent<Button>().onClick.AddListener(UnlockMirageAttack);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockBloodsucker();
        UnlockMirageAttack();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockBloodsucker()
    {
        if (blodsuckerUnlockButton.unlocked)
            blodsuckerUnlocked = true;
    }

    private void UnlockMirageAttack()
    {
        if (mirrageAttackButton.unlocked)
            mirrageAttackUnlocked = true;
    }

    public void MakeMirageOnParry(Transform spawnTransform)
    {
        if (mirrageAttackUnlocked)
            SkillManager.Instance.CloneSkill.CreateCloneWithDelay(spawnTransform);
    }
}

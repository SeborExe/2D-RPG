using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private SkillTreeSlotUI unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked { get; private set; }

    [Header("Mirage Dodge")]
    [SerializeField] private SkillTreeSlotUI unlockMirageDodgeButton;
    public bool dodgeMirageUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked)
        {
            player.CharacterStats.Evasion.AddModifiers(evasionAmount);
            Inventory.Instance.InvokeOnItemEquiped();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.Instance.CloneSkill.CreateClone(player.transform, new Vector2(2 * player.FacingDir, 0));
        }
    }
}

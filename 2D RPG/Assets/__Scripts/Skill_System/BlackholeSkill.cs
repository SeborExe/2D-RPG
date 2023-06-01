using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private SkillTreeSlotUI blackHoleUnlockButton;
    public bool blackHoleUnlocked { get; private set; }
    [Space]

    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholeDuration;

    [Space]
    [SerializeField] private int attacksAmount;
    [SerializeField] private float cloneAttackCooldown;

    BlackholeSkillController currentBlackhole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();
        currentBlackhole.SetUpBlackhole(maxSize, growSpeed, shrinkSpeed, attacksAmount, cloneAttackCooldown, blackholeDuration, player);
    }

    protected override void Start()
    {
        base.Start();

        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckUnlock()
    {
        UnlockBlackHole();
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole) return false;

        if (currentBlackhole.PlayerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    private void UnlockBlackHole()
    {
        if (blackHoleUnlockButton.unlocked)
            blackHoleUnlocked = true;
    }
}

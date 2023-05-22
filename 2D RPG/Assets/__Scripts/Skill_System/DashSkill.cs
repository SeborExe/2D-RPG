using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private SkillTreeSlotUI dashUnlockButton;

    [Header("Clone on Dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private SkillTreeSlotUI cloneOnDashUnlockButton;

    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private SkillTreeSlotUI cloneOnArrivalUnlockButton;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    private void OnDestroy()
    {
        dashUnlockButton.GetComponent<Button>().onClick.RemoveListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.RemoveListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.RemoveListener(UnlockCloneOnArrival);
    }

    private void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }
    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }
    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
            SkillManager.Instance.CloneSkill.CreateClone(player.transform, Vector3.zero);
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.Instance.CloneSkill.CreateClone(player.transform, Vector3.zero);
    }
}

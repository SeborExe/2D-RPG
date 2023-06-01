using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private MainGameUI mainGameUI;

    public bool unlocked;

    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    [SerializeField, TextArea] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;
    private Image skillImage;

    private Button button;

    private void OnValidate()
    {
        gameObject.name = $"SkillSlot - {skillName}";
    }

    private void Awake()
    {
        skillImage = GetComponent<Image>();
        button = GetComponent<Button>();
        mainGameUI = GetComponentInParent<MainGameUI>();

        button.onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        skillImage.color = lockedSkillColor;

        if (unlocked)
            skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (!PlayerManager.Instance.HaveEnoughMoney(skillPrice))
            return;

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
                return;
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
                return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mainGameUI.skillTooltipUI.ShowTooltip(skillName, skillDescription, skillPrice.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mainGameUI.skillTooltipUI.HideTooltip();
    }

    public void LoadData(GameData data)
    {
        if (data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.skillTree.TryGetValue(skillName, out bool value))
        {
            data.skillTree.Remove(skillName);
            data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            data.skillTree.Add(skillName, unlocked);
        }
    }
}

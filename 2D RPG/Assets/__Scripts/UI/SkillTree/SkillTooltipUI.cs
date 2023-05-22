using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTooltipUI : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillDescription;
    [SerializeField] private TMP_Text skillPrice;

    public void ShowTooltip(string name, string description, string skillPrice)
    {
        skillName.text = name;
        skillDescription.text = description;
        this.skillPrice.text = skillPrice;

        gameObject.SetActive(true);
    }

    public void HideTooltip() => gameObject.SetActive(false);
}

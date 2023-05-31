using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTooltipUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statName;
    [SerializeField] private TMP_Text statDescription;

    public void ShowStatTooltip(string statName, string description)
    {
        this.statName.text = statName;
        statDescription.text = description;

        gameObject.SetActive(true);
    }

    public void HideTooltip() => gameObject.SetActive(false);
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string statName;
    [SerializeField, TextArea] private string statDescription;

    [SerializeField] private StatType statType;
    [SerializeField] private TMP_Text statNameText;
    [SerializeField] private TMP_Text statValueText;

    private MainGameUI mainGameUI;

    private void OnValidate()
    {
        gameObject.name = $"Stat - {statName}";

        if (statNameText != null)
            statNameText.text = statName;
    }

    private void Start()
    {
        mainGameUI = GetComponentInParent<MainGameUI>();

        UpdateStatValueUI();
        Inventory.Instance.OnItemEquiped += UpdateStatValueUI;
    }

    private void OnDisable()
    {
        Inventory.Instance.OnItemEquiped -= UpdateStatValueUI;
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

            if (statType == StatType.Health)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();

            if (statType == StatType.Damage)
                statValueText.text = playerStats.GetDamage().ToString();

            if (statType == StatType.CritPower)
                statValueText.text = (playerStats.CriticPower.GetValue() + playerStats.Strength.GetValue()).ToString();

            if (statType == StatType.CritChance)
                statValueText.text = (playerStats.CriticChance.GetValue() + playerStats.Agility.GetValue()).ToString();

            if (statType == StatType.Evasion)
                statValueText.text = (playerStats.Evasion.GetValue() + playerStats.Agility.GetValue()).ToString();

            if (statType == StatType.MaicRes)
                statValueText.text = (playerStats.MagicResistance.GetValue() + (playerStats.Intelligence.GetValue() * 3)).ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mainGameUI.statTooltipUI.ShowStatTooltip(statName, statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mainGameUI.statTooltipUI.HideTooltip();
    }
}

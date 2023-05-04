using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatSlotUI : MonoBehaviour
{
    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    [SerializeField] private TMP_Text statNameText;
    [SerializeField] private TMP_Text statValueText;

    private void OnValidate()
    {
        gameObject.name = $"Stat - {statName}";

        if (statNameText != null)
            statNameText.text = statName;
    }

    private void Start()
    {
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
        }
    }
}

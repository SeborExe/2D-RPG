using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private Slider healthSlider;
    private CharacterStats stats;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponentInParent<RectTransform>();
        healthSlider = GetComponentInChildren<Slider>();
        stats = GetComponentInParent<CharacterStats>();
    }

    private void OnEnable()
    {
        entity.OnFlipped += Entity_OnFlipped;
        stats.OnHealthChanged += Stats_OnHealthChanged;
    }

    private void OnDisable()
    {
        entity.OnFlipped -= Entity_OnFlipped;
        stats.OnHealthChanged -= Stats_OnHealthChanged;
    }

    private void Entity_OnFlipped()
    {
        rectTransform.Rotate(0, 180f, 0);
    }

    private void Stats_OnHealthChanged()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthSlider.maxValue = stats.GetMaxHealthValue();
        healthSlider.value = stats.CurrentHealth;

        if (stats.CurrentHealth == 0)
            gameObject.SetActive(false);
    }
}

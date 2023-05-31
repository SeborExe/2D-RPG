using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerManager playerManager;
    private Player player;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text currencyText;

    [Header("Skills Cooldown Images")]
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;

    private void Start()
    {
        playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        playerManager = PlayerManager.Instance;
        player = playerManager.player;
        skills = SkillManager.Instance;

        playerStats.OnHealthChanged += UpdateHealthUI;

        player.OnDashUsed += Player_OnDashUsed;
        player.OnParryUsed += Player_OnParryUsed;
        player.OnCrystalUsed += Player_OnCrystalUsed;
        player.OnSwordUsed += Player_OnSwordUsed;
        player.OnBlackholeUsed += Player_OnBlackholeUsed;
        player.OnFlaskUsed += Player_OnFlaskUsed;

        playerManager.OnCurrencyChanged += UpdateCurrency;

        UpdateHealthUI();
        UpdateCurrency();
    }

    private void Update()
    {
        CheckCooldownOf(dashImage, skills.DashSkill.cooldown);
        CheckCooldownOf(parryImage, skills.ParrySkill.cooldown);
        CheckCooldownOf(crystalImage, skills.CrystalSkill.cooldown);
        CheckCooldownOf(swordImage, skills.SwordSkill.cooldown);
        CheckCooldownOf(blackholeImage, skills.BlackholeSkill.cooldown);
        CheckCooldownOf(flaskImage, Inventory.Instance.flaskCooldown);
    }

    private void UpdateHealthUI()
    {
        hpSlider.maxValue = playerStats.GetMaxHealthValue();
        hpSlider.value = playerStats.CurrentHealth;
    }

    private void SetCooldownOf(Image image)
    {
        if (image.fillAmount <= 0)
        {
            image.fillAmount = 1;
        }
    }

    private void CheckCooldownOf(Image image, float cooldown)
    {
        if (image.fillAmount > 0)
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
    }

    private void UpdateCurrency()
    {
        currencyText.text = $"Souls: {playerManager.Currency}";
    }

    private void Player_OnDashUsed() => SetCooldownOf(dashImage);

    private void Player_OnParryUsed() => SetCooldownOf(parryImage);

    private void Player_OnCrystalUsed() => SetCooldownOf(crystalImage);

    private void Player_OnSwordUsed() => SetCooldownOf(swordImage);

    private void Player_OnBlackholeUsed() => SetCooldownOf(blackholeImage);

    private void Player_OnFlaskUsed() => SetCooldownOf(flaskImage);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canExplode;

    private GameObject currentCrystal;

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            CrystalSkillController crystal = currentCrystal.GetComponent<CrystalSkillController>();
            crystal.SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed);
        }
        else
        {
            Vector2 playerPos = player.transform.position;

            player.transform.position = currentCrystal.transform.position;

            currentCrystal.transform.position = playerPos;
            currentCrystal.GetComponent<CrystalSkillController>().FinishCrystal();
        }
    }
}

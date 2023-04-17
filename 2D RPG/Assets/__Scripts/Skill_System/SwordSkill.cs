using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Info")]
    [SerializeField] private SwordSkillController swordPrefab;
    [SerializeField] private Vector2 lunchDirection;
    [SerializeField] private float swordGravity;

    public void CreateSword()
    {
        SwordSkillController newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        newSword.SetUpSword(lunchDirection, swordGravity);
    }
}

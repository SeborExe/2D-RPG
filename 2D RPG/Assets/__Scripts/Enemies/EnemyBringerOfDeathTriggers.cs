using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBringerOfDeathTriggers : EnemyAnimationTrigger
{
    private EnemyBringerOfDeath enemyBringer => GetComponentInParent<EnemyBringerOfDeath>();

    private void Relocate() => enemyBringer.FindPosition();

    private void MakeInvisible() => enemyBringer.EntityFX.MakeTransparent(true);
    private void MakeVisible() => enemyBringer.EntityFX.MakeTransparent(false);
}

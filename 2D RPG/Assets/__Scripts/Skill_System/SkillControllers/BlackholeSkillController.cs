using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;

    private bool canGrow = true;
    private bool canShrink;
    private bool cloneAttackReleased;
    private bool canCreateHotKeys = true;
    private bool playerCanDisapear = true;

    private int attacksAmount;
    private float cloneAttackCooldown;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKeys = new List<GameObject>();

    private Player player;

    public bool PlayerCanExitState { get; private set; }

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList = new List<KeyCode>();

    public void SetUpBlackhole(float maxSize, float growSpeed, float shrinkSpeed, int attacksAmount, float cloneAttackCooldown, float blackholeDuration,  Player player)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.attacksAmount = attacksAmount;
        this.cloneAttackCooldown = cloneAttackCooldown;
        this.player = player;

        blackholeTimer = blackholeDuration;

        if (SkillManager.Instance.CloneSkill.GetCrystalInsteadOfClone())
            playerCanDisapear = false;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer <= 0)
        {
            blackholeTimer = Mathf.Infinity;

            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        //Only or testing//////
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
        ///////////////////////

        CloneAttackLogic();
        ChangeHolSize();
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0) return;

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            player.EntityFX.MakeTransparent(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy) && !enemy.IsDead)
        {
            enemy.FreezTime(true);

            SetUpHotKey(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.FreezTime(false);
        }
    }

    private void SetUpHotKey(Enemy enemy)
    {
        if (keyCodeList.Count <= 0) return;

        if (!canCreateHotKeys) return;

        KeyCode randomKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(randomKey);

        GameObject newHotKey = Instantiate(hotKeyPrefab, enemy.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKeys.Add(newHotKey);

        newHotKey.GetComponent<BlackholeHotKeyController>().SetupHotKey(randomKey, enemy.transform, this);
    }

    private void ChangeHolSize()
    {
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer <= 0 && cloneAttackReleased && attacksAmount > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex;
            float xOffset;
            GetRandomIndexAndOffset(out randomIndex, out xOffset);

            if (SkillManager.Instance.CloneSkill.GetCrystalInsteadOfClone())
            {
                float radius = maxSize / 2;
                SkillManager.Instance.CrystalSkill.CreateCrystal();
                SkillManager.Instance.CrystalSkill.CurrentCrystalChooseRandonTarget(radius);
            }
            else
            {
                SkillManager.Instance.CloneSkill.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            }

            attacksAmount--;

            if (attacksAmount <= 0)
            {
                float delayAfterAbilityFinish = 1f;
                Invoke(nameof(FinishBlackholeAbility), delayAfterAbilityFinish);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();
        PlayerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotKeys()
    {
        if (createdHotKeys.Count <= 0) return;

        for (int i = 0; i < createdHotKeys.Count; i++)
        {
            Destroy(createdHotKeys[i]);
        }
    }

    public void AddEnemyToList(Transform enemy)
    {
        targets.Add(enemy);
    }

    private void GetRandomIndexAndOffset(out int randomIndex, out float xOffset)
    {
        randomIndex = Random.Range(0, targets.Count);

        if (Random.Range(0, 100) > 50)
            xOffset = 2;
        else
            xOffset = -2;
    }
}

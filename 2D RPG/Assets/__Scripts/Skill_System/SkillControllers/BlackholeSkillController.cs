using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    private List<Transform> targets = new List<Transform>();

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList = new List<KeyCode>();

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            //targets.Add(enemy.transform);
            enemy.FreezTime(true);

            SetUpHotKey(enemy);
        }
    }

    private void SetUpHotKey(Enemy enemy)
    {
        if (keyCodeList.Count <= 0) return;

        KeyCode randomKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(randomKey);

        GameObject newHotKey = Instantiate(hotKeyPrefab, enemy.transform.position + new Vector3(0, 2), Quaternion.identity);
        newHotKey.GetComponent<BlackholeHotKeyController>().SetupHotKey(randomKey, enemy.transform, this);
    }

    public void AddEnemyToList(Transform enemy)
    {
        targets.Add(enemy);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotKeyController : MonoBehaviour
{
    private KeyCode myHotKey;
    private TMP_Text myText;
    private SpriteRenderer spriteRenderer;

    private Transform myEnemy;
    private BlackholeSkillController myBlackhole;

    public void SetupHotKey(KeyCode hotKey, Transform myEnemy, BlackholeSkillController myBlackhole)
    {
        myText = GetComponentInChildren<TMP_Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        this.myEnemy = myEnemy;
        this.myBlackhole = myBlackhole;

        myHotKey = hotKey;
        myText.text = myHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            myBlackhole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }
}

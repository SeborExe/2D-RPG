using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Info")]
    [SerializeField] private SwordSkillController swordPrefab;
    [SerializeField] private Vector2 lunchForce;
    [SerializeField] private float swordGravity;

    private Vector2 finalDirection;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    public void CreateSword()
    {
        SwordSkillController newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        newSword.SetUpSword(finalDirection, swordGravity);

        DotsActive(false);
    }

    protected override void Start()
    {
        base.Start();

        GenerateDots();
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            finalDirection = new Vector2(AimDirection().normalized.x * lunchForce.x, AimDirection().normalized.y * lunchForce.y);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * lunchForce.x,
            AimDirection().normalized.y * lunchForce.y) * t * 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
}

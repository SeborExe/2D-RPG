using System;
using UnityEngine;
using UnityEngine.UI;

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Skill Info")]
    [SerializeField] private SkillTreeSlotUI swordUnlockButton;
    public bool swordUnlocked { get; private set; }
    [SerializeField] private SwordSkillController swordPrefab;
    [SerializeField] private Vector2 lunchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Bounce Info")]
    [SerializeField] private SkillTreeSlotUI bounceUnlockButton;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private SkillTreeSlotUI pierceUnlockButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private SkillTreeSlotUI spinUnlockButton;
    [SerializeField] private float hitCooldown;
    [SerializeField] private float maxTravelDinstance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinMoveSpeed;
    [SerializeField] private float spinGravity;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    [Header("Pasive Skills")]
    [SerializeField] private SkillTreeSlotUI timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private SkillTreeSlotUI vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }

    private GameObject[] dots;
    private Vector2 finalDirection;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
        SetUpGravity();

        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpin);
    }

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockBounce();
        UnlockSpin();
        UnlockPierce();
        UnlockTimeStop();
        UnlockVulnerable();
    }

    public void CreateSword()
    {
        SwordSkillController newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        newSword.SetUpSword(finalDirection, swordGravity, player, swordType, freezTimeDuration, returnSpeed);

        if (swordType == SwordType.Bounce)
            newSword.SetUpBounce(bounceAmount, bounceSpeed);

        else if (swordType == SwordType.Pirce)
            newSword.SetUpPierce(pierceAmount);

        else if (swordType == SwordType.Spin)
            newSword.SetUpSpin(maxTravelDinstance, spinDuration, hitCooldown, spinMoveSpeed);

        player.AssignNewSword(newSword.gameObject);

        DotsActive(false);
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

    private void SetUpGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pirce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    private void UnlockSword()
    {
        if (swordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }

    private void UnlockVulnerable()
    {
        if (vulnerableUnlockButton.unlocked)
            vulnerableUnlocked = true;
    }

    private void UnlockBounce()
    {
        if (bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }

    private void UnlockPierce()
    {
        if (pierceUnlockButton.unlocked)
            swordType = SwordType.Pirce;
    }

    private void UnlockSpin()
    {
        if (spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }

    #region Aim
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
            AimDirection().normalized.y * lunchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}

using Cinemachine;
using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Player player;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;
    [SerializeField] private float flashDuration = 0.2f;

    [Header("Ailments Color")]
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] schockColors;
    [SerializeField] private Color[] igniteColors;

    [Header("FX")]
    [SerializeField] private GameObject igniteFX;
    [SerializeField] private GameObject chillFX;
    [SerializeField] private GameObject shockFX;
    [SerializeField] private GameObject hitFX00;
    [SerializeField] private GameObject criticalHitFX;

    [Space]
    [SerializeField] private ParticleSystem dustFX;
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;

    [Header("Camera Shake")]
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpack;
    public Vector3 shakeHighDamage;
    public Vector3 shakeAttack;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMat = spriteRenderer.material;
        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (TryGetComponent(out Player playerComponent))
            player = playerComponent;
    }

    private void Update()
    {
        if (afterImageCooldownTimer > 0)
        {
            afterImageCooldownTimer -= Time.deltaTime;
            if (afterImageCooldownTimer < 0) afterImageCooldownTimer = 0;
        }
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
            spriteRenderer.color = Color.clear;
        else
            spriteRenderer.color = Color.white;
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer <= 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetUpAfterImage(colorLooseRate, spriteRenderer.sprite);
        }
    }

    public IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMat;
    }

    public void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    public void IgniteFX(float seconds, float blinkingDuration = 0.2f)
    {
        GameObject FX = Instantiate(igniteFX, transform.position, Quaternion.identity);
        Destroy(FX, seconds);

        InvokeRepeating(nameof(IgniteColorFX), 0, blinkingDuration);
        Invoke(nameof(CancelColorChange), seconds);
    }

    public void ChillFX(float seconds, float blinkingDuration = 0.2f)
    {
        GameObject FX = Instantiate(chillFX, transform.position, Quaternion.identity);
        Destroy(FX, seconds);

        InvokeRepeating(nameof(ChillColorFX), 0, blinkingDuration);
        Invoke(nameof(CancelColorChange), seconds);
    }

    public void SchockFX(float seconds, float blinkingDuration = 0.2f)
    {
        GameObject FX = Instantiate(shockFX, transform.position, Quaternion.identity);
        Destroy(FX, seconds);

        InvokeRepeating(nameof(SchockColorFX), 0, blinkingDuration);
        Invoke(nameof(CancelColorChange), seconds);
    }

    private void IgniteColorFX()
    {
        if (spriteRenderer.color != igniteColors[0])
            spriteRenderer.color = igniteColors[0];
        else
            spriteRenderer.color = igniteColors[1];
    }

    private void SchockColorFX()
    {
        if (spriteRenderer.color != schockColors[0])
            spriteRenderer.color = schockColors[0];
        else
            spriteRenderer.color = schockColors[1];
    }

    private void ChillColorFX()
    {
        if (spriteRenderer.color != chillColor)
            spriteRenderer.color = chillColor;
        else
            spriteRenderer.color = chillColor;
    }

    private void SetColorBlinking(Color colorToSet)
    {
        spriteRenderer.color = colorToSet;
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }

    public void CreateHitFX(Transform hitPosition, bool isCritical)
    {
        float randomZRotation = Random.Range(-90f, 90f);
        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);

        Vector3 hitRotation = new Vector3(0, 0, randomZRotation);

        GameObject hitFxPrefab = hitFX00;

        if (isCritical)
        {
            hitFxPrefab = criticalHitFX;

            float yRotation = 0;
            randomZRotation = Random.Range(-45f, 45f);

            if (GetComponent<Entity>().FacingDir == -1)
                yRotation = 180;

            hitRotation = new Vector3(0, yRotation, randomZRotation);
        }

        GameObject hitFX = Instantiate(hitFxPrefab, hitPosition.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        hitFX.transform.Rotate(hitRotation);

        Destroy(hitFX, 1f);
    }

    public void PlayDustFX()
    {
        if (dustFX != null)
            dustFX.Play();
    }

    public void ScreenShake(Vector3 shakePower)
    {
        impulseSource.m_DefaultVelocity = new Vector3(shakePower.x * player.FacingDir, shakePower.y) * shakeMultiplier;
        impulseSource.GenerateImpulse();
    }
}

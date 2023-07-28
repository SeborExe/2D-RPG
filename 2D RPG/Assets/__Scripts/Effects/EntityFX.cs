using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Player player;

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

    [Header("Popup")]
    [SerializeField] private GameObject popupTextPrefab;

    private GameObject myHealthBar;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        originalMat = spriteRenderer.material;
        myHealthBar = GetComponentInChildren<HealthBarUI>().gameObject;

        if (TryGetComponent(out Player playerComponent))
            player = playerComponent;
    }

    public void CreatePopupText(string text, bool isDamageText = false)
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(2f, 4f);
        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popupTextPrefab, transform.position + positionOffset, Quaternion.identity);
        newText.GetComponent<TMP_Text>().text = text;

        if (isDamageText)
            newText.GetComponent<TMP_Text>().color = Color.red;
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
        {
            spriteRenderer.color = Color.clear;
            myHealthBar.SetActive(false);
        }
        else
        {
            spriteRenderer.color = Color.white;
            myHealthBar.SetActive(true);
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
}

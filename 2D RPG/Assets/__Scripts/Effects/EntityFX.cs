using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

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

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMat = spriteRenderer.material;
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
            spriteRenderer.color = Color.clear;
        else
            spriteRenderer.color = Color.white;
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
}

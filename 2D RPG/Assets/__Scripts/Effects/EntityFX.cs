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
    [SerializeField] private int flashDurationInMiliseconds = 200;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMat = spriteRenderer.material;
    }

    public async Task FlashFX()
    {
        spriteRenderer.material = hitMat;

        await Task.Delay(flashDurationInMiliseconds);

        spriteRenderer.material = originalMat;
    }

    public void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    public void CancelReadBlink()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}

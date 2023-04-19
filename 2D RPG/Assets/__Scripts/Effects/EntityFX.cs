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

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMat = spriteRenderer.material;
    }

    public IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

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

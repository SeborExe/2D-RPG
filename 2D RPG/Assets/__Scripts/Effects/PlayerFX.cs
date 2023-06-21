using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Space, Header("After Image")]
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

    protected override void Start()
    {
        base.Start();

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (afterImageCooldownTimer > 0)
        {
            afterImageCooldownTimer -= Time.deltaTime;
            if (afterImageCooldownTimer < 0) afterImageCooldownTimer = 0;
        }
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

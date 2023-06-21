using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private float sfxMinimumDistance;

    public bool playBgm;

    private int bgmIndex;
    private bool canPlaySFX;

    protected override void Awake()
    {
        base.Awake();

        Invoke(nameof(AllowSFX), 1f);
    }

    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PLayBGM(bgmIndex);
        }
    }

    public void PlaySFX(int sfxIndex, Transform source)
    {
        if (!canPlaySFX) return;

        if (source != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, source.position) > sfxMinimumDistance)
            return;

        if (sfxIndex < sfx.Length)
        {
            sfx[sfxIndex].pitch = Random.Range(0.85f, 1.15f);
            sfx[sfxIndex].Play();
        }
    }

    public void StopSFX(int sfxIndex) => sfx[sfxIndex].Stop();

    public void StopSFXWithTime(int index, float decreasingSpeed = 0.5f)
    {
        StartCoroutine(DecreaseVolume(sfx[index], decreasingSpeed));
    }

    private IEnumerator DecreaseVolume(AudioSource audioSource, float decreasingSpeed)
    {
        float defaultVolume = audioSource.volume;

        while (audioSource.volume > 0.1f)
        {
            audioSource.volume -= audioSource.volume * 0.2f;
            yield return new WaitForSeconds(decreasingSpeed);

            if (audioSource.volume <= 0.1f)
            {
                audioSource.Stop();
                audioSource.volume = defaultVolume;
                break;
            }
        }
    }

    public void PLayBGM(int bgmIndex)
    {
        this.bgmIndex = bgmIndex;
        StopAllBGM();
        bgm[this.bgmIndex].Play();
    }

    public void PLayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PLayBGM(bgmIndex);
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}

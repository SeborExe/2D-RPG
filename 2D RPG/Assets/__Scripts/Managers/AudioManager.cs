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

    protected override void Awake()
    {
        base.Awake();
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
        //if (sfx[sfxIndex].isPlaying) return;

        if (source != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, source.position) > sfxMinimumDistance)
            return;

        if (sfxIndex < sfx.Length)
        {
            sfx[sfxIndex].pitch = Random.Range(0.85f, 1.15f);
            sfx[sfxIndex].Play();
        }
    }

    public void StopSFX(int sfxIndex) => sfx[sfxIndex].Stop();

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
}

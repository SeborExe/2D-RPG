using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreenUI : MonoBehaviour
{
    private readonly int FADE_OUT = Animator.StringToHash("FadeOut");
    private readonly int FADE_IN = Animator.StringToHash("FadeIn");

    [SerializeField] private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeOut() => anim.SetTrigger(FADE_OUT);
    public void FadeIn() => anim.SetTrigger(FADE_IN);
}

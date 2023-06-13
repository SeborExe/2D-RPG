using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;

    public string checkpointID;
    public bool activated;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.AddCheckpointToList(this);
    }

    [ContextMenu("Generate checkpoint ID")]
    private void GenerateID()
    {
        checkpointID = Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player) && !activated)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        AudioManager.Instance.PlaySFX(5, null);

        activated = true;
        anim.SetBool(Resources.Active, true);
    }
}

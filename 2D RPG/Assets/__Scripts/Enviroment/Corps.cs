using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corps : MonoBehaviour
{
    public int Currency { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            PlayerManager.Instance.AddCurrency(Currency);
            Destroy(gameObject);
        }
    }
}

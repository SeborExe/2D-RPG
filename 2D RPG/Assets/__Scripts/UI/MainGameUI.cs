using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameUI : MonoBehaviour
{
    [field: SerializeField] public ItemTooltipUI itemTooltipUI { get; private set; }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
            menu.SetActive(true);
    }
}

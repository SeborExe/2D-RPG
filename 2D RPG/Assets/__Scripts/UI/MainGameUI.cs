using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameUI : MonoBehaviour
{
    [field: SerializeField] public ItemTooltipUI itemTooltipUI { get; private set; }
    [field: SerializeField] public StatTooltipUI statTooltipUI { get; private set; }

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;

    private void Start()
    {
        SwitchTo(null);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchWithKeyTo(optionsUI);
    }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
            menu.SetActive(true);
    }

    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            return;
        }

        SwitchTo(menu);
    }
}

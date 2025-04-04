using System;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    [SerializeField] private GameObject controlConfigMenu;

    private void Start()
    {
        controlConfigMenu.gameObject.SetActive(false);
    }

    public void toggleMenu()
    {
        if (!controlConfigMenu)
        {
            controlConfigMenu.gameObject.SetActive(true);
        }
        else
        {
            controlConfigMenu.gameObject.SetActive(false);
        }
    }
}

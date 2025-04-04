using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Weapons : MonoBehaviour
{
    public Shooting[] Guns = new Shooting[2];
    public int Equip;
    public TextMeshPro EquipUI;
    public Animator PlayerAnim;

    [SerializeField] PlayerInput playerControllerInput; //this is only intended for checking player controller input, hence the naming
    
    void Start()
    {
        PlayerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerBaseScript.isControllerConnected)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (i <= Guns.Length && i > 0)
                {
                    if (Keyboard.current[Key.Digit1 + (i - 1)].wasPressedThisFrame) //this checks for keyboard input directly, no rebind
                    {
                        Equip = i - 1;

                    }
                }
            }
        }
        if (PlayerBaseScript.isControllerConnected)
        {
            int i = Equip; // Start from the currently equipped weapon
            if (playerControllerInput.actions["Weapon Cycle"].triggered)
            {
                i++;
                if (i >= Guns.Length)
                {
                    Equip = 0; // Cycle back to the first weapon
                }
                else
                {
                    Equip = i;
                }
            }
        }

        for (int i = 0; i < Guns.Length; i++)
        {
            if (i == Equip)
            {
                Guns[i].enabled = true;
            }
            else
            {
                Guns[i].enabled = false;
            }
        }

        if (Guns[Equip].GunName == "Revolver")
        {
            PlayerAnim.SetBool("RevolverEquip", true);
        }
        else
        {
            PlayerAnim.SetBool("RevolverEquip", false);
        }

        if (Guns[Equip].GunName == "SMG")
        {
            PlayerAnim.SetBool("SMGEquip", true);
        }
        else
        {
            PlayerAnim.SetBool("SMGEquip", false);
        }

        if (Guns[Equip].GunName == "Shotgun")
        {
            PlayerAnim.SetBool("ShotgunEquip", true);
        }
        else
        {
            PlayerAnim.SetBool("ShotgunEquip", false);
        }

        if (Guns[Equip].GunName == "Rifle")
        {
            PlayerAnim.SetBool("RifleEquip", true);
        }
        else
        {
            PlayerAnim.SetBool("RifleEquip", false);
        }
        
        EquipUI.text = ($"{Equip + 1}- {Guns[Equip].GunName}");

    }
}

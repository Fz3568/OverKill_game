using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    //todo
    //add pause menu once game is mostly done
    [SerializeField] private InputAction pauseGame;
    [SerializeField] private Canvas canvas;//pause menu gose here
   
    [SerializeField] PlayerInput playerInput;
  
    public static bool isPaused;
    private void OnEnable()
    {
        pauseGame.Enable();
    }

    private void OnDisable()
    {
        pauseGame.Disable();
    }
    
    void Start()
    {
        pauseGame.performed += _ => Pause();
    }

    private void Pause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            playerInput.DeactivateInput();//thanks sequential computing, otherwise you can still fire a shot before game actually pauses
            Time.timeScale = 0;
            //Cursor.visible = true; //will enable this once menu is done
            canvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            playerInput.ActivateInput();
            //Cursor.visible = false; //same
            isPaused = false;
            canvas.enabled = false;
        }
    }
}
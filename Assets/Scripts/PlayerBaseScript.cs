using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBaseScript : MonoBehaviour
{
    //float XInput;
    // float YInput;// check later
    public bool IsDodging;

    public float speed;
    public float DodgeDuration;
    public float DodgeSpeed;
    public float DodgeDelay;
    float DodgeTime;
    float DodgeStop;
    ParticleSystem particles;

    public Color DashColor;

    float kbf; //Kickback float

    public Camera cam;

    Vector2 Movement;
    Vector2 MousePos;

    public GameObject gunPoint;
    public AudioSource DashSound;

    Rigidbody2D rb;

    [SerializeField] PlayerInput playerInput;

    public static bool isControllerConnected;

    private int MaxHP = 100;
    int hp = 100;

    public Slider HpBar;
    
    public GameObject GameOver;
    private bool died;

    private void Awake()
    {
        checkForController();
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
        IsDodging = false;
        DodgeTime = Time.time;
        kbf = 0;
    }

    // Update is called once per frame
    void Update()
    {
        checkForController();
        
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);   // there's a "Look" thing that keeps track of mouse position in the new input system,
                                                                    // not sure how it works, will figure out, maybe

        if (playerInput.actions["Dash"].triggered && DodgeTime < Time.time)
        {
            IsDodging = true;
            speed = speed * DodgeSpeed;
            DodgeTime = Time.time + DodgeDelay + DodgeDuration;
            DodgeStop = Time.time + DodgeDuration;
            gameObject.GetComponent<SpriteRenderer>().color = DashColor;
            particles.Play();
            DashSound.Play();
        }
        
        if (IsDodging == true && DodgeStop < Time.time)
        {
            IsDodging = false;
            speed = speed / DodgeSpeed;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            particles.Stop();
            DashSound.Stop();
        }

        HpBar.value = hp;
        
        if (died)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(1);
            }
        }
       
    }

    private void FixedUpdate()
    {
        //seperate keyboard and mouse input and controller input
        //tho movement are the same, so no changes there, but for aiming, there's difference
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Movement = new Vector2(input.x, input.y) * (speed * Time.deltaTime);
        rb.MovePosition(Movement + new Vector2(transform.position.x, transform.position.y) + new Vector2(transform.up.x, transform.up.y) * kbf); //movement + kickback if shot
        kbf = 0;
        
        
        if (isControllerConnected)
        {
            //me no understand maths works how
            //so basically when lookdirection is at 0, rotation of 0 degrees is staight up, which is fine
            //but as soon as there's input from stick, direction gets all messed up unless you rotate them -90 degrees
            //so I just made it so that when there's no input, it undose the -90 degrees adjustment
            //this way of structuring code also means it supports hot swap on input devices
            
            Vector2 lookDirection = playerInput.actions["Look"].ReadValue<Vector2>(); //this spits out a normalized directional vector, thanks unity
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f; // Converts to degrees
            
            if (lookDirection.x == 0.00f && lookDirection.y == 0.00f)    
            {
                angle += 90f;
                rb.rotation = angle;
            }
            else
            {
                rb.rotation = angle;
            }
        }
        else if (!isControllerConnected)
        {
            Vector2 LookDir = rb.position - MousePos;
            float angle = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg + 90f;
            rb.rotation = angle;
        }
    }

    public void Kickback(float kb)
    {
        kbf = -kb;
    }

    private void checkForController()
    {
        if (Gamepad.current != null)
        {
            isControllerConnected = true;
        }
        else
        {
            isControllerConnected = false;
        }
    }

    public float getPlayerHealth()
    {
        return hp;
    }

    public void TakeDamage(int damage)
    {
        if (IsDodging)
        {
            return;
        }
        else
        {
            hp -= damage;
            CancelInvoke("Heal");
            Invoke("Heal", 4f);
            if (hp < 1)
            {
                Die();
            }
        }
    }
    public void Die()
    {
        Time.timeScale = 0;
        GameOver.SetActive(true);
        GameOver.GetComponent<WinTrigger>().turnOff();
        died = true;
        
    }
    void Heal()
    {
        if (hp < MaxHP)
        {
            hp += 1;
        }
        Invoke("Heal", 0.04f);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    //we can probably get away with using polymorphism right, by creating two masterclasses of "single action weapon" and "automatic weapon"
    
    // Start is called before the first frame update
    public string GunName;
    public float BulletForce;
    public float Innacuracy;
    public float ShotDelay;
    public float Recoil;
    public float Kickback;
    public int maxAmmo;
    public bool isAutomatic;
    public int shots;
    public float BulletLifetime;
    public float SpeedVariance;


    public Transform FirePoint;
    public GameObject bulletPrefab;
    public ParticleSystem Flash;
    public GameObject ReloadUIpos;
    public GameObject ReloadUI;
    public Animator ReloadAnim;
    public AudioSource GunShot;
    public AudioSource ReloadOpen;
    public AudioSource ReloadClose;
    AudioSource LoadBullet;

    int LoadedAmmo;
    float shootTime;
    private bool isInReloadPrep; //renamed this to make it more consistent with internal control action naming, used to be called "ReloadState"
    public float NormalSpeed;
    public Animator animator;

    public PlayerBaseScript Base;
    
    [SerializeField] PlayerInput playerInput;
    private float isHoldingDownFire;// this value is derived from:
                                    // isHoldingDownFire = playerInput.actions["Fire"].ReadValue<float>().
                                    // It will spit out either 0.0 (not pressed down) or 1.0 (pressed down) BUTTONS ONLY, JOYSTICK WILL PRODUCE NUMBER FROM 0.0 TO 1.0 DEPENDS ON TRAVEL DISTANCE
                                    // Basically a bool but with no true or false
                                    // There's also a .performed action type, not sure difference between the two, documentation did not make it clear

    private void OnEnable()
    {
        isInReloadPrep = false;
    }

    private void OnDisable()
    {
        if (ReloadUIpos != null)
        {
            ReloadUI.GetComponent<RectTransform>().position = new Vector2(ReloadUIpos.transform.position.x, -20 + ReloadUIpos.transform.position.y);
        }
    }

    private void Start()
    {
        LoadedAmmo = maxAmmo;
        shootTime = Time.time;
        LoadBullet = gameObject.GetComponent<AudioSource>();
        isInReloadPrep = false;
        animator = gameObject.GetComponent<Animator>();
        Base = gameObject.GetComponent<PlayerBaseScript>();
    }

    // Update is called once per frame
    void Update()
    {
        ReloadAnim.SetInteger("Ammo", LoadedAmmo);

        isHoldingDownFire = playerInput.actions["Fire"].ReadValue<float>();
        
        
        if (playerInput.actions["Reload Prep"].triggered)
        {
            if (!isInReloadPrep && isAutomatic == false)
            {
                isInReloadPrep = true;
                ReloadOpen.Play();
            }
            else if (isInReloadPrep && isAutomatic == false)
            {
                isInReloadPrep = false;
                ReloadClose.Play();
            }
            else if (IsInvoking("EndReloadSMG") == false) 
            {
                isInReloadPrep = true;
                LoadBullet.volume = 0.2f;
                LoadBullet.pitch = 1.2f;
                LoadedAmmo = maxAmmo;
                Invoke("EndReloadSMG", 0.35375f);
            }
            
        }
        
        // tbh makes no sense
        // if (Input.GetAxis("Mouse ScrollWheel") > 0 && ReloadState == false)
        // {
        //     ReloadState = true;
        //     ReloadOpen.Play();
        // }
        // if (Input.GetAxis("Mouse ScrollWheel") < 0 && ReloadState == true)
        // {
        //     ReloadState = false;
        //     ReloadClose.Play();
        // } 


        if (isInReloadPrep)
        {
            animator.SetBool("IsReloading", true);
            Base.speed = NormalSpeed * 0.6f;
            
            if (!isAutomatic)
            {
                if (playerInput.actions["Reload"].triggered && LoadedAmmo < maxAmmo)
                {
                    LoadBullet.volume = 0.2f;
                    LoadBullet.pitch = 1.2f;
                    LoadedAmmo = LoadedAmmo + 1;
                    LoadBullet.Play();
                }
            }
            
            ReloadUI.GetComponent<RectTransform>().position = ReloadUIpos.transform.position;
        }
        else
        {
            animator.SetBool("IsReloading", false);
            if (Base.IsDodging == false)
            {
                Base.speed = NormalSpeed;
            }

            if (playerInput.actions["Fire"].triggered && LoadedAmmo == 0)
            {
                LoadBullet.pitch = 2f;
                LoadBullet.volume = 0.1f;
                LoadBullet.Play();
            }

            if (((playerInput.actions["Fire"].triggered && !isAutomatic) || (isHoldingDownFire > 0.0f && isAutomatic)) && LoadedAmmo > 0 && shootTime < Time.time)
            {
                Shoot();
            }

            ReloadUI.GetComponent<RectTransform>().position = new Vector2(ReloadUIpos.transform.position.x, -20 + ReloadUIpos.transform.position.y);
        }
        
    }

    private void Shoot()
    {
        for (int i = 1; i <= shots; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            if (shots > 1)
            {
                bullet.transform.localScale = bullet.transform.localScale * 0.7f;
            }
            bulletScript.Weapon = GunName;
            bulletRB.AddForce((FirePoint.up + Random.Range(-Innacuracy, Innacuracy) * FirePoint.right) * (BulletForce + Random.Range(-SpeedVariance, +SpeedVariance)), ForceMode2D.Impulse);
            Destroy(bullet, BulletLifetime);            
        }
        
        Flash.Play();
        GunShot.pitch = Random.Range(0.85f, 0.95f);
        GunShot.Play();
        Innacuracy = Innacuracy * Recoil;
        Invoke("RegainAccuracy", 0.3f);

        

        LoadedAmmo = LoadedAmmo - 1;
        shootTime = Time.time + ShotDelay;

        Base.Kickback(Kickback);
    }
    void RegainAccuracy()
    {
        Innacuracy = Innacuracy / Recoil;
    }

    void EndReloadSMG()
    {
        isInReloadPrep = false;
        ReloadClose.Play();
    }
}

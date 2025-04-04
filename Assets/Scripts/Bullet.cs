using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string Weapon = "";
    int damage = 0;
    Quaternion StartRotation;
    public GameObject DeathSplash;
    void Start()
    {
        StartRotation = transform.rotation;        
    }

    // Update is called once per frame
    void Update()
    {
        if (Weapon == "Revolver")
        {
            damage = 12;
        }
        if (Weapon == "Shotgun")
        {
            damage = 4;
        }
        if (Weapon == "SMG")
        {
            damage = 2;
        }
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.gameObject.tag == "Enemy")
        {
            Color OGcolor = collision.collider.gameObject.GetComponent<SpriteRenderer>().color;
            ParticleSystem ps = collision.collider.GetComponentInChildren<ParticleSystem>();
            ps.transform.rotation = new Quaternion(0, 0, StartRotation.z + Random.Range(-0.2f, 0.2f), Quaternion.identity.w);
            ps.gameObject.transform.position = transform.position;
            ps.Play();

            if (collision.gameObject.GetComponent<Spitter>() != null)
            {
                Spitter enemy = collision.gameObject.GetComponent<Spitter>();
                enemy.hp -= damage;
                if (enemy.hp < 1)
                {
                    GameObject splash = Instantiate(DeathSplash, enemy.transform.position, Quaternion.identity);
                    Destroy(splash, 5f);
                    Destroy(enemy.gameObject);
                }
            }

            if (collision.gameObject.GetComponent<Ram>() != null)
            {
                Ram enemy = collision.gameObject.GetComponent<Ram>();
                enemy.hp -= damage;
                if (enemy.hp < 1)
                {
                    GameObject splash = Instantiate(DeathSplash, enemy.transform.position, Quaternion.identity);
                    Destroy(splash, 5f);
                    Destroy(enemy.gameObject);
                }
            }
        }
        
            Destroy(gameObject);
        
        
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
        [SerializeField] float speed = 8f;
        [SerializeField] float life = 2f;

        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            Destroy(gameObject, life);
            rb.linearVelocity = transform.up * speed;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    
}

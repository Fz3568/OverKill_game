using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acidblob : MonoBehaviour
{
    [SerializeField] private GameObject acidPuddlePrefab;
    [SerializeField] private float lifetime = 2f;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBaseScript>().TakeDamage(25);
            Destroy(gameObject);
            return;
        }
        if (other.CompareTag("Wall"))
        {
            SpawnPuddle();
            Destroy(gameObject);
        }
    }

    private void SpawnPuddle()
    {
        Instantiate(acidPuddlePrefab, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        SpawnPuddle();
    }
}


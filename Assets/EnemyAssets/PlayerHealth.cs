using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    public void InstantKill()
    {
        currentHealth = 0;
        Die();
    }

    public void Die()
    {
        Debug.Log("Player Has Been Slayed!");
        gameObject.SetActive(false);
    }
}

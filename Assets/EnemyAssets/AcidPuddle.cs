using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPuddle : MonoBehaviour
{
    [SerializeField] private float damagePercentage = 0.25f;
    [SerializeField] private float lifetime = 5f;

    private List<PlayerHealth> playersInPuddle = new List<PlayerHealth>();

    private void Start()
    {
        Destroy(gameObject, lifetime);
        InvokeRepeating(nameof(ApplyDamage), 0f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null && !playersInPuddle.Contains(health))
            {
                playersInPuddle.Add(health);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null && playersInPuddle.Contains(health))
            {
                playersInPuddle.Remove(health);
            }
        }
    }

    private void ApplyDamage()
    {
        List<PlayerHealth> activePlayers = new List<PlayerHealth>(playersInPuddle);
        
        foreach (PlayerHealth health in activePlayers)
        {
            if (health != null)
            {
                int damage = Mathf.CeilToInt(health.MaxHealth * damagePercentage);
                health.TakeDamage(damage);
            }
            else
            {
                playersInPuddle.Remove(health);
            }
        }
    }
}

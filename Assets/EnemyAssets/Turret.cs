using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 200f; 
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float maxHeat = 100f;
    [SerializeField] private float heatPerShot = 15f;
    [SerializeField] private float cooldownRate = 25f;
    [SerializeField] private float cooldownDelay = 2f;

    private Transform playerTarget;
    private float fireCooldown;
    private float currentHeat;
    private bool overheated;
    private float cooldownTimer;
    private bool wasFiring;

    private void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        HandleHeat();

        if (!overheated && playerTarget != null)
        {
            if (IsPlayerInRange() && HasLineOfSight())
            {
                RotateTurret();

                if (Time.time >= fireCooldown && IsAimedAtTarget())
                {
                    FireProjectile();
                }
            }
        }
    }

    private bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, playerTarget.position) <= targetingRange;
    }

    private bool HasLineOfSight()
    {
        Vector2 direction = (Vector2)playerTarget.position - (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetingRange, obstacleLayer);
        return hit.collider == null || hit.collider.CompareTag("Player");
    }

    private void RotateTurret()
    {
        Vector2 direction = (Vector2)playerTarget.position - (Vector2)transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
    }

    private bool IsAimedAtTarget()
    {
        Vector2 directionToPlayer = ((Vector2)playerTarget.position - (Vector2)transform.position).normalized;
        float angleDifference = Vector2.Angle(transform.right, directionToPlayer);
        return angleDifference < 5f;
    }

    private void HandleHeat()
    {
        if (overheated)
        {
            currentHeat -= cooldownRate * Time.deltaTime;
            if (currentHeat <= 0)
            {
                overheated = false;
                currentHeat = 0;
            }
            return;
        }

        if (Time.time < fireCooldown)
        {
            cooldownTimer = 0;
            wasFiring = true;
        }
        else
        {
            if (wasFiring)
            {
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer >= cooldownDelay)
                {
                    currentHeat -= cooldownRate * Time.deltaTime;
                    currentHeat = Mathf.Max(currentHeat, 0);
                }
            }
            wasFiring = false;
        }
    }

    private void FireProjectile()
    {
        if (overheated)
        {
            return;
        }

        fireCooldown = Time.time + 1f / fireRate;
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        currentHeat += heatPerShot;
        if (currentHeat >= maxHeat)
        {
            overheated = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);

        if (playerTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, playerTarget.position);
        }
    }
}
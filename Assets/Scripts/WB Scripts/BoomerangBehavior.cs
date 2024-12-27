using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehavior : WeaponBehavior
{
    private int additionalProjectiles = -1; // Start at -1 for base projectile
    private float currentTravelDistance;

    protected override void Start()
    {
        base.Start();
        currentTravelDistance = weaponData.travelRange; // Initialize travel range from WeaponData
    }

    public override void Attack(Vector2 direction, float playerSpeed)
    {
        int totalProjectiles = 1 + additionalProjectiles; // Base projectile + additional projectiles
        float angleStep = 360f / totalProjectiles; // Angle between projectiles

        for (int i = 0; i < totalProjectiles; i++)
        {
            // Calculate the direction for each projectile
            float angle = angleStep * i;
            Vector2 angledDirection = Quaternion.Euler(0, 0, angle) * direction;

            // Instantiate the boomerang
            GameObject boomerang = Instantiate(weaponData.prefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = boomerang.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                StartCoroutine(HandleBoomerang(rb, firePoint.position, angledDirection.normalized, playerSpeed));
            }
        }
    }

    private IEnumerator HandleBoomerang(Rigidbody2D rb, Vector2 startPosition, Vector2 direction, float playerSpeed)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Collider2D boomerangCollider = rb.GetComponent<Collider2D>();

            if (playerCollider != null && boomerangCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, boomerangCollider);
            }
        }

        Vector2 travelPoint = startPosition + direction * currentTravelDistance;
        rb.angularVelocity = weaponData.rotationSpeed; // Add rotation

        // Travel outward
        while (Vector2.Distance(rb.position, travelPoint) > 0.1f)
        {
            rb.velocity = direction * (weaponData.Speed + playerSpeed * 0.5f);
            yield return null;
        }

        // Travel back toward the player's current position
        while (player != null && Vector2.Distance(rb.position, (Vector2)player.transform.position) > 0.1f)
        {
            Vector2 currentPlayerPosition = player.transform.position;
            Vector2 returnDirection = (currentPlayerPosition - rb.position).normalized;
            rb.velocity = returnDirection * (weaponData.Speed + playerSpeed * 0.5f);
            yield return null;
        }

        // Destroy the boomerang after returning
        Destroy(rb.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            DoDamage(collision);
        }
    }

    protected override void OnLevelUp(int newLevel)
    {
        if (newLevel % 2 == 1)
        {
            // Add a new projectile
            additionalProjectiles = Mathf.Min(additionalProjectiles + 1, 3); // Cap at 4 projectiles
            Debug.Log($"Boomerang leveled up! Added an additional projectile. Total: {1 + additionalProjectiles}");
        }
        else
        {
            // Double the travel distance
            currentTravelDistance *= 2;
            Debug.Log($"Boomerang leveled up! Travel distance increased to {currentTravelDistance}");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBehavior : WeaponBehavior
{
    private int hitCount = 0;
    private int additionalProjectiles = -1;


    public override void Attack(Vector2 direction, float playerSpeed)
    {
       
        int totalProjectiles = 1 + additionalProjectiles; // Base projectile + additional projectiles
        float angleStep = 360f / totalProjectiles; // Angle between projectiles

        for (int i = 0; i < totalProjectiles; i++)
        {
            // Calculate the direction for each projectile
            float angle = angleStep * i;
            Vector2 angledDirection = Quaternion.Euler(0, 0, angle) * direction;

            // Instantiate the axe
            GameObject axe = Instantiate(weaponData.prefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = axe.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = angledDirection.normalized * (weaponData.Speed + playerSpeed * 0.5f);
                rb.angularVelocity = weaponData.rotationSpeed;
            }
        }

    }

    private void Update()
    {
        // Check if the axe has traveled beyond its range
        if (Vector2.Distance(firePoint.position, (Vector2)transform.position) >= weaponData.travelRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            hitCount++;

            // Perform damage on the enemy
            DoDamage(collision);

            if (hitCount >= 3)
            {
                Destroy(gameObject);  // Destroy the axe after hitting 3 enemies
            }
        }
    }
    protected override void OnLevelUp(int newLevel)
    {
        // Customize leveling logic
        if (newLevel == 2)
        {
            additionalProjectiles++;
        }
        else if (newLevel <= 8)
        {
            additionalProjectiles++;
        }
    }
}

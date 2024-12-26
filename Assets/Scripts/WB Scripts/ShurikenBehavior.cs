using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenBehavior : WeaponBehavior
{

    private int additionalProjectiles = -1;

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

    public override void Attack(Vector2 direction, float playerSpeed)
    {

        int totalProjectiles = 1 + additionalProjectiles; // Base projectile + additional projectiles
        float angleStep = 360f / totalProjectiles; // Angle between projectiles

        for (int i = 0; i < totalProjectiles; i++)
        {
            // Calculate the direction for each projectile
            float angle = angleStep * i;
            Vector2 angledDirection = Quaternion.Euler(0, 0, angle) * direction;

            // Instantiate the shuriken
            GameObject shuriken = Instantiate(weaponData.prefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = shuriken.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = angledDirection.normalized * (weaponData.Speed + playerSpeed * 0.5f);
                rb.angularVelocity = weaponData.rotationSpeed;
            }            
        }
    }


    private void Update()
    {
        // Check if the shuriken has traveled beyond its range
        if (Vector2.Distance(firePoint.position, (Vector2)transform.position) >= weaponData.travelRange)
        {
            
            Destroy(gameObject);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            DoDamage(collision); // Apply damage to the enemy

            // Destroy the shuriken after it hits an enemy
            Destroy(gameObject);
        }
        // Do nothing if the shuriken collides with anything other than an enemy
    }
}

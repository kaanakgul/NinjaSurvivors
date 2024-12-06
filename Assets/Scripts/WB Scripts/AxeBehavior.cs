using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AxeBehavior : WeaponBehavior
    {
        private int hitCount = 0;
        

        public override void Attack(Vector2 direction, float playerSpeed)
        {
            // Instantiate the axe projectile
            GameObject axe = Instantiate(weaponData.prefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = axe.GetComponent<Rigidbody2D>();

            // Set the velocity of the axe based on the player's speed and the direction
            rb.velocity = direction.normalized * (weaponData.Speed + playerSpeed * 0.5f);

            // Apply rotation for the spinning effect (optional)
            rb.angularVelocity = weaponData.rotationSpeed;

           
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
    { return; }
    }



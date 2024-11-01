using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    //public float speed = 10f; // Speed of the projectile (if applicable)
    private Vector2 direction; // Direction the weapon will travel (for ranged)
    public float rotationSpeed = 360f; // Speed of rotation in degrees per second
    public bool isRanged; // Indicates if the weapon is ranged
    public float damageAmount = 50f; // Damage dealt by the weapon
    public float cooldown = 0.5f; // Cooldown duration for this weapon
    private float lastFireTime; // Last time this weapon was fired
    public WeaponData weaponData;

    Rigidbody2D rb;

    private void Awake()
    {
        // Initialize any required components here
    }

    private void Start()
    {
        lastFireTime = Time.time; // Initialize the last fire time
        if (isRanged)
        {
            Destroy(gameObject, 5f); // Destroy ranged weapon after 5 seconds
        }

        rb = GetComponent<Rigidbody2D>();
    }

    // Method to set the direction of the weapon (for ranged)
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized; // Normalize the direction
    }

    // Check if the weapon can be fired
    public bool CanFire()
    {
        return Time.time >= lastFireTime + cooldown; // Check if enough time has passed
    }

    // Reset the last fire time when the weapon is fired
    public void ResetFireTime()
    {
        lastFireTime = Time.time; // Update last fire time
    }

    private void FixedUpdate()
    {
        // Move the projectile in the set direction (only for ranged)
        if (isRanged && direction != Vector2.zero)
        {
            if (rb != null)
            {
                rb.velocity = direction * weaponData.Speed; // Apply velocity
                transform.Rotate(0, 0, -rotationSpeed * Time.fixedDeltaTime); // Rotate
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Call the doDamage function to apply damage to the enemy
            DoDamage(collision);

            // Destroy the weapon after hitting the enemy (only for ranged)
            if (isRanged)
            {
                Destroy(gameObject);
            }
        }
    }

    // Method to deal damage to the enemy
    private void DoDamage(Collider2D enemy)
    {
        EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
        if (enemyBehaviour != null)
        {
            enemyBehaviour.TakeDamage(damageAmount); // Apply damage
        }
    }

    // Optional: Handle visibility for melee weapons
    public IEnumerator HandleVisibility()
    {
        // Logic to show the weapon (animation, etc.)
        // Example: Set the weapon's active state or visibility
        yield return null; // Add your visibility logic here
    }
}

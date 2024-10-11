using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenBehavior : MonoBehaviour
{
    public float speed = 10f; // Speed of the shuriken
    private Vector2 direction; // Direction the shuriken will travel
    public float rotationSpeed = 360f; // Speed of shuriken rotation in degrees per second
    private Rigidbody2D rb; // Rigidbody2D reference for physics-based movement

    public float damageAmount = 50f; // Damage the shuriken will deal to enemies

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, 5f); // Destroy the shuriken after 5 seconds to avoid clutter
    }

    // Method to set the direction of the shuriken
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized; // Ensure the direction is normalized
    }

    private void FixedUpdate()
    {
        // Move the shuriken in the set direction using Rigidbody2D
        if (direction != Vector2.zero)
        {
            rb.velocity = direction * speed;

            // Rotate the shuriken around its Z-axis to make it spin clockwise
            transform.Rotate(0, 0, -rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Call the doDamage function to apply damage to the enemy
            doDamage(collision);

            // Destroy the shuriken after hitting the enemy
            Destroy(gameObject);
        }
    }

    // Method to deal damage to the enemy
    private void doDamage(Collider2D enemy)
    {
        // Get the EnemyBehaviour component from the collided object
        EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();

        // If the enemy has an EnemyBehaviour component, apply damage
        if (enemyBehaviour != null)
        {
            enemyBehaviour.TakeDamage(damageAmount); // Apply 50 damage to the enemy's health
        }
    }
}

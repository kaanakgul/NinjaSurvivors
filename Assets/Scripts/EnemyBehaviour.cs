using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform targetDestination; // Reference to the player's Transform
    [SerializeField] private float speed = 2f; // Speed of enemy movement
    [SerializeField] private float damage = 10f; // Damage dealt to the player
    public float attackSpeed = 0.5f; // Time between attacks
    private bool attacking = false; // Is the enemy currently attacking?
    private Coroutine attackRoutine; // To store the attack coroutine
    private Rigidbody2D rb; // Rigidbody2D for movement

    // Enemy stats
    public float health = 100f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
    }

    private void FixedUpdate()
    {
        if (targetDestination != null)
        {
            Vector3 direction = (targetDestination.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    // Method to set the target (player)
    public void SetTarget(Transform playerTransform)
    {
        targetDestination = playerTransform;
    }

    // Method to set enemy stats (called from EnemySpawner)
    public void SetStats(float newHealth, float newSpeed, float newDamage)
    {
        health = newHealth;
        speed = newSpeed;
        damage = newDamage;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the enemy is colliding with the player
        if (collision.gameObject.CompareTag("Player") && !attacking)
        {
            // Start the damage coroutine if not already attacking
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
            }

            attackRoutine = StartCoroutine(DealDamage(collision.gameObject));
            attacking = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Stop attacking when the enemy exits collision with the player
        if (collision.gameObject.CompareTag("Player") && attacking)
        {
            StopCoroutine(attackRoutine);
            attacking = false;
            attackRoutine = null;
        }
    }

    // Coroutine for dealing damage to the player
    private IEnumerator DealDamage(GameObject player)
    {
        Debug.Log("Starting to deal damage to player");
        Charachter characterComponent = player.GetComponent<Charachter>();

        // Make sure the player has a Charachter component and isn't dead
        if (characterComponent != null && !characterComponent.isDead)
        {
            while (!characterComponent.isDead)
            {
                // Reduce player's health
                characterComponent.health -= damage;
                Debug.Log("Player takes " + damage + " damage.");
                Debug.Log("Player has " + characterComponent.health + " health remaining.");

                // Wait for the attack speed duration
                yield return new WaitForSeconds(attackSpeed);
            }
        }

        attacking = false; // Ensure attacking state resets if the player is dead
    }

    // Example of taking damage to the enemy itself
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle enemy death
    private void Die()
    {
        Destroy(gameObject); // Destroy enemy object on death
    }
}

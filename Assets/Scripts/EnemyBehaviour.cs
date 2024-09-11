using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform targetDestination; // Reference to the player's Transform
    [SerializeField] float speed = 2f; // Speed of enemy movement
    [SerializeField] float damage = 10f; // Damage dealt to the player
    public float attackSpeed = 0.5f; // Time between attacks
    private bool attacking = false; // Is the enemy currently attacking?
    private Coroutine attackRoutine; // To store the attack coroutine
    private Rigidbody2D rb; // Rigidbody2D for movement

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the enemy is colliding with the player
        if (collision.gameObject.CompareTag("Player") && !attacking)
        {
            // Start the damage coroutine if not already attacking
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
        }
    }

    // Coroutine for dealing damage to the player
    private IEnumerator DealDamage(GameObject player)
    {
        Debug.Log("Starting to deal damage to player");  // Add this for debugging
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
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform targetDestination; // Reference to the player's Transform
    [SerializeField] private float speed; // Speed of enemy movement
    [SerializeField] private float damage; // Damage dealt to the player
    public float attackSpeed = 0.5f; // Time between attacks
    private bool attacking = false; // Is the enemy currently attacking?
    private Coroutine attackRoutine; // To store the attack coroutine
    private Rigidbody2D rb; // Rigidbody2D for movement

    // Enemy stats
    public float health = 100f;
    public EnemyData enemyData; // Reference to enemy data scriptable object

    public Animator animator;
    public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
    }

    private void FixedUpdate()
    {
        if (targetDestination != null)
        {
            Vector3 direction = (targetDestination.position - transform.position).normalized;
            navMeshAgent.SetDestination(targetDestination.position);

            // Call the animation method to update based on movement direction
            UpdateAnimations(direction);
        }
    }

    // Method to set the target (player)
    public void SetTarget(Transform playerTransform)
    {
        targetDestination = playerTransform;
    }

    // Method to set enemy stats (called from EnemySpawner)
    public void SetStats(float newHealth, float newSpeed, float newDamage, float newAttackSpeed)
    {
        health = newHealth;
        speed = newSpeed;
        damage = newDamage;
        attackSpeed = newAttackSpeed;
    }

    private void Start()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
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
        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        // Make sure the player has a PlayerStats component and isn't dead
        if (playerStats != null && !playerStats.IsDead)
        {
            while (!playerStats.IsDead)
            {
                // Reduce player's health
                playerStats.TakeDamage(damage);
                Debug.Log("Player takes " + damage + " damage.");
                Debug.Log("Player has " + playerStats.currentHealth + " health remaining.");

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

    // Handle enemy death and apply rewards
    private void Die()
    {
        // Generate random gold and exp rewards within defined range
        int goldReward = Random.Range(enemyData.minGold, enemyData.maxGold + 1);
        int expReward = Random.Range(enemyData.minExp, enemyData.maxExp + 1);
        FindObjectOfType<PlayerStats>().IncrementKillCount();


        // Give rewards to the player
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.AddGold(goldReward);
            playerStats.AddExperience(expReward);
        }

        Destroy(gameObject); // Destroy enemy object on death
    }

    // New method to handle animation updates based on movement direction
    private void UpdateAnimations(Vector3 direction)
    {
        if (animator != null)
        {
            // Reset all movement booleans
            animator.SetBool("WalkUp", false);
            animator.SetBool("WalkDown", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkLeft", false);

            // Determine which direction the enemy is moving and trigger the corresponding animation
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    // Moving Right
                    animator.SetBool("WalkRight", true);
                }
                else
                {
                    // Moving Left
                    animator.SetBool("WalkLeft", true);
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    // Moving Up
                    animator.SetBool("WalkUp", true);
                }
                else
                {
                    // Moving Down
                    animator.SetBool("WalkDown", true);
                }
            }
        }
    }
}

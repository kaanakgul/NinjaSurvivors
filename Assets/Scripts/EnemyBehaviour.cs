using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Animator animator;
    private Transform target; // Target (e.g., player)

    private float currentHealth;

    public EnemyData enemyData; // Enemy stats from ScriptableObject

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InitializeEnemy();
    }

    private void FixedUpdate()
    {
        if (target != null && navMeshAgent != null)
        {
            // Move towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            navMeshAgent.SetDestination(target.position);

            // Update animations based on direction
            UpdateAnimations(direction);
        }
    }

    /// <summary>
    /// Sets the enemy data and initializes its stats.
    /// </summary>
    public void SetEnemyData(EnemyData data)
    {
        enemyData = data;
        InitializeEnemy();
    }

    /// <summary>
    /// Sets the target for the enemy to chase.
    /// </summary>
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    /// <summary>
    /// Initializes enemy stats from the assigned EnemyData.
    /// </summary>
    private void InitializeEnemy()
    {
        if (enemyData != null)
        {
            currentHealth = enemyData.health;

            if (navMeshAgent != null)
            {
                navMeshAgent.speed = enemyData.speed;
                navMeshAgent.updateRotation = false;
                navMeshAgent.updateUpAxis = false;
            }
        }
        else
        {
            Debug.LogError("EnemyData is not assigned!");
        }
    }

    /// <summary>
    /// Applies damage to the enemy and handles death if health reaches zero.
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles enemy death, rewards the player, and destroys the enemy.
    /// </summary>
    private void Die()
    {
        // Reward the player with gold and experience
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            int goldReward = Random.Range(enemyData.minGold, enemyData.maxGold + 1);
            int expReward = Random.Range(enemyData.minExp, enemyData.maxExp + 1);
            playerStats.AddGold(goldReward);
            playerStats.AddExperience(expReward);
            playerStats.IncrementKillCount();
        }

        Destroy(gameObject); // Destroy the enemy game object
    }

    /// <summary>
    /// Updates movement animations based on the direction of travel.
    /// </summary>
    private void UpdateAnimations(Vector3 direction)
    {
        if (animator != null)
        {
            animator.SetBool("WalkUp", direction.y > 0 && Mathf.Abs(direction.y) > Mathf.Abs(direction.x));
            animator.SetBool("WalkDown", direction.y < 0 && Mathf.Abs(direction.y) > Mathf.Abs(direction.x));
            animator.SetBool("WalkRight", direction.x > 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y));
            animator.SetBool("WalkLeft", direction.x < 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y));
        }
    }
}

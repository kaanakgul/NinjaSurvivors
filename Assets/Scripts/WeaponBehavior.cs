using System.Collections;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public WeaponData weaponData; // Reference to weapon data for stats
    private Vector2 direction; // Direction for ranged weapons
    private Rigidbody2D rb; // Rigidbody for ranged weapons
    private float lastFireTime; // Time tracking for cooldowns
    private int currentLevel = 1; // Current weapon level
    private bool isMelee => !weaponData.isRanged; // Check if weapon is melee

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Initialize Rigidbody
    }

    private void Start()
    {
        lastFireTime = Time.time;
        if (weaponData.isRanged)
        {
            Destroy(gameObject, 10f); // Destroy ranged projectiles after a set duration
        }
    }

    // Set the direction for ranged weapons
    public void SetDirection(Vector2 dir, float playerSpeed)
    {
        direction = dir.normalized;
        rb.velocity = direction * (weaponData.Speed + playerSpeed * 0.5f); // Adjust speed
    }

    // Check if the weapon can fire based on its cooldown
    public bool CanFire()
    {
        float adjustedCooldown = weaponData.cooldown * PlayerStats.Instance.attackSpeedMultiplier;
        return Time.time >= lastFireTime + adjustedCooldown;
    }

    // Reset fire cooldown after firing
    public void ResetFireTime()
    {
        lastFireTime = Time.time;
    }

    // Melee attack visibility and logic
    public void PerformMeleeAttack(Vector2 attackDirection)
    {
        Debug.Log($"Performing melee attack with {weaponData.weaponName}");

        // Position the melee weapon
        transform.position = (Vector3)attackDirection + transform.parent.position;
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, attackDirection));

        // Enable weapon visibility
        gameObject.SetActive(true);

        // Detect enemies within attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, weaponData.range);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                var enemyBehavior = enemy.GetComponent<EnemyBehaviour>();
                if (enemyBehavior != null)
                {
                    float adjustedDamage = weaponData.damageAmount * PlayerStats.Instance.damageMultiplier;
                    enemyBehavior.TakeDamage(adjustedDamage);
                    Debug.Log($"Hit {enemy.name} for {adjustedDamage} damage.");
                }
            }
        }

        // Schedule hiding after attack
        StartCoroutine(HideAfterAttack(weaponData.cooldown));
    }

    // Handle visibility hiding after a melee attack
    private IEnumerator HideAfterAttack(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (weaponData.isRanged && direction != Vector2.zero)
        {
            float adjustedSpeed = weaponData.Speed / PlayerStats.Instance.attackSpeedMultiplier;
            rb.velocity = direction * adjustedSpeed; // Adjust velocity
            transform.Rotate(0, 0, -weaponData.rotationSpeed * Time.fixedDeltaTime); // Rotate projectile
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            DoDamage(collision);

            if (weaponData.isRanged)
            {
                Destroy(gameObject);
            }
        }
    }

    // Deal damage to enemies
    private void DoDamage(Collider2D enemy)
    {
        var enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
        if (enemyBehaviour != null)
        {
            float adjustedDamage = weaponData.damageAmount * PlayerStats.Instance.damageMultiplier;
            enemyBehaviour.TakeDamage(adjustedDamage);
        }
    }

    // Handle weapon level-ups dynamically
    public void LevelUp(int newLevel)
    {
        if (newLevel <= currentLevel || newLevel > 8) return; // Validate level
        Debug.Log($"{weaponData.weaponName} leveled up to {newLevel}!");
        currentLevel = newLevel;
    }

    public int GetCurrentLevel() => currentLevel; // Get current weapon level
}

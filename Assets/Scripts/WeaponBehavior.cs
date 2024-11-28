using System.Collections;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public WeaponData weaponData; // Reference to weapon data for stats
    private Vector2 direction; // Direction for ranged weapons
    private Rigidbody2D rb; // Rigidbody for ranged weapons
    private float lastFireTime; // Time tracking for cooldowns
    private int currentLevel = 1; // Current weapon level

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Initialize Rigidbody
    }

    private void Start()
    {
        lastFireTime = Time.time;
        if (weaponData.isRanged)
        {
            Destroy(gameObject, 10f); // Destroy ranged projectiles after 5 seconds
        }
    }

    // Set the direction for ranged weapons
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
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

    private void FixedUpdate()
    {
        if (weaponData.isRanged && direction != Vector2.zero)
        {
            float adjustedSpeed = weaponData.Speed / PlayerStats.Instance.attackSpeedMultiplier;
            rb.velocity = direction * adjustedSpeed; // Adjust velocity
            transform.Rotate(0, 0, -weaponData.rotationSpeed * Time.    fixedDeltaTime); // Rotate projectile
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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

    // Handle visibility for melee weapons
    public IEnumerator HandleVisibility()
    {
        gameObject.SetActive(true); // Enable visibility
        yield return new WaitForSeconds(0.5f); // Adjust duration as needed
        gameObject.SetActive(false); // Hide after attack
    }

    // Handle weapon level-ups dynamically
    public void LevelUp(int newLevel)
    {
        if (newLevel <= currentLevel || newLevel > 8) return; // Validate level

        Debug.Log($"{weaponData.weaponName} leveled up to {newLevel}!");

        currentLevel = newLevel;
    }

    // Get the current level of the weapon
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}

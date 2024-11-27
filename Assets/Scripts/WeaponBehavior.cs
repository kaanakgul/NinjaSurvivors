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
            Destroy(gameObject, 5f); // Destroy ranged projectiles after 5 seconds
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
        return Time.time >= lastFireTime + weaponData.cooldown;
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
            rb.velocity = direction * weaponData.Speed; // Set velocity
            transform.Rotate(0, 0, -weaponData.rotationSpeed * Time.fixedDeltaTime); // Rotate projectile
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
            enemyBehaviour.TakeDamage(weaponData.damageAmount);
        }
    }

    // Handle visibility for melee weapons
    public IEnumerator HandleVisibility()
    {
        Debug.Log($"Handling visibility for {weaponData.weaponName}");

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

        // Adjust weapon stats based on the level
        weaponData.damageAmount += 5f * (newLevel - 1);
        weaponData.cooldown *= 0.9f;

        if (weaponData.isRanged)
        {
            weaponData.Speed += 1f * (newLevel - 1);
        }
    }

    // Get the current level of the weapon
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}

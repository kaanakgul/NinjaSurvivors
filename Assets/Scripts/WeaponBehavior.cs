using System.Collections;
using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    public WeaponData weaponData; // Reference to weapon data
    private float lastFireTime; // Time tracking for cooldowns
    private int currentLevel = 1; // Current weapon level

    protected Transform firePoint; // Point where weapons spawn projectiles

    protected virtual void Start()
    {
        if (firePoint == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                firePoint = player.transform.Find("FirePoint");
                if (firePoint == null)
                {
                    Debug.LogError("FirePoint not found as a child of the Player.");
                }
            }
            else
            {
                Debug.LogError("Player object not found in the scene.");
            }
        }
    }

    public bool CanFire()
    {
        float adjustedCooldown = weaponData.cooldown * PlayerStats.Instance.attackSpeedMultiplier;
        return Time.time >= lastFireTime + adjustedCooldown;
    }

    public void ResetFireTime()
    {
        lastFireTime = Time.time;
    }

    public void DoDamage(Collider2D target)
    {
        if (target.CompareTag("Enemy"))
        {
            var enemyBehaviour = target.GetComponent<EnemyBehaviour>();
            if (enemyBehaviour != null)
            {
                // Calculate damage based on weapon data and player stats
                float adjustedDamage = weaponData.damageAmount * PlayerStats.Instance.damageMultiplier;

                // Apply damage to the enemy
                enemyBehaviour.TakeDamage(adjustedDamage);

                // Apply lifesteal if applicable
                PlayerStats.Instance.HealFromLifesteal(adjustedDamage);

                Debug.Log($"Dealt {adjustedDamage} damage to {target.name}.");
            }
        }
        else return;
    }

    public void LevelUp(int newLevel)
    {
        if (newLevel <= currentLevel || newLevel > 8) return;
        Debug.Log($"{weaponData.weaponName} leveled up to {newLevel}!");
        currentLevel = newLevel;
        OnLevelUp(newLevel); // Call subclass-specific logic
    }

    public int GetCurrentLevel() => currentLevel;

    // Abstract method for custom attack logic
    public abstract void Attack(Vector2 direction, float playerSpeed);

    protected abstract void OnLevelUp(int newLevel); // Abstract method for unique logic



}

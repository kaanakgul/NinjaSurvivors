using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponBehavior> activeWeapons = new List<WeaponBehavior>(); // List of currently active weapons
    public Transform firePoint; // Where weapons (projectiles, etc.) will spawn from
    private Vector2 currentFireDirection = Vector2.right; // Default fire direction

    private void Start()
    {
        StartCoroutine(FireWeapons()); // Start the firing loop for active weapons
    }

    // Method to add a new weapon to the active weapon list
    public void AddWeapon(WeaponBehavior newWeapon)
    {
        if (!activeWeapons.Contains(newWeapon))
        {
            activeWeapons.Add(newWeapon);
        }
    }

    // Upgrades an existing weapon (increase damage, fire rate, etc.)
    public void UpgradeWeapon(WeaponBehavior weapon)
    {
        weapon.damageAmount *= 1.5f; // Example: Increase damage by 50%
        weapon.cooldown *= 0.9f; // Example: Decrease cooldown by 10%
    }

    // Sets the direction for firing ranged weapons
    public void SetFireDirection(Vector2 direction)
    {
        currentFireDirection = direction; // Update firing direction
    }

    // Automatically fire all active weapons
    private IEnumerator FireWeapons()
    {
        while (true)
        {
            foreach (WeaponBehavior weapon in activeWeapons)
            {
                if (weapon.CanFire()) // Check if the weapon can fire
                {
                    FireWeapon(weapon); // Fire the weapon
                    weapon.ResetFireTime(); // Reset fire time after firing
                }
            }
            yield return null; // Wait for the next frame (can adjust timing here)
        }
    }

    // Fires a single weapon
    private void FireWeapon(WeaponBehavior weapon)
    {
        if (weapon.isRanged)
        {
            // Instantiate and fire ranged weapon projectiles
            WeaponBehavior projectile = Instantiate(weapon, firePoint.position, Quaternion.identity);
            projectile.SetDirection(currentFireDirection); // Set direction for the projectile
        }
        else
        {
            // Trigger melee weapon logic (like swinging or proximity checks)
            StartCoroutine(weapon.HandleVisibility()); // Show melee weapon during the attack
        }
    }

    // Optional: Add logic for weapon removal, cooldown management, etc.
}

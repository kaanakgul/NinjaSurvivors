using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponData> weaponDataList; // List to hold weapon data
    public Transform firePoint; // Where weapons (projectiles, etc.) will spawn from
    private Vector2 currentFireDirection = Vector2.right; // Default fire direction
    private List<WeaponBehavior> activeMeleeWeapons = new List<WeaponBehavior>(); // List of active melee weapons

    private void Start()
    {
        // Initialize melee weapons from weapon data, skipping projectiles
        foreach (var weaponData in weaponDataList)
        {
            if (!weaponData.isRanged) // Only add melee weapons to activeMeleeWeapons
            {
                WeaponBehavior weapon = Instantiate(weaponData.prefab, transform).GetComponent<WeaponBehavior>();
                weapon.damageAmount = weaponData.damageAmount;
                weapon.cooldown = weaponData.cooldown;
                weapon.isRanged = weaponData.isRanged;
                activeMeleeWeapons.Add(weapon);
            }
        }

        StartCoroutine(FireWeapons()); // Start the firing loop for melee weapons
        StartCoroutine(FireRangedWeapons()); // Start the firing loop for ranged weapons
    }

    // Sets the direction for firing ranged weapons
    public void SetFireDirection(Vector2 direction)
    {
        currentFireDirection = direction; // Update firing direction
    }

    // Automatically fire all active melee weapons
    private IEnumerator FireWeapons()
    {
        while (true)
        {
            foreach (WeaponBehavior weapon in activeMeleeWeapons)
            {
                if (weapon != null && weapon.CanFire()) // Check if the weapon exists and can fire
                {
                    FireMeleeWeapon(weapon); // Fire the melee weapon
                    weapon.ResetFireTime(); // Reset fire time after firing
                }
            }
            yield return null; // Wait for the next frame (can adjust timing here)
        }
    }

    // Automatically fire ranged weapons based on their cooldowns
    private IEnumerator FireRangedWeapons()
    {
        while (true)
        {
            foreach (var weaponData in weaponDataList)
            {
                if (weaponData.isRanged)
                {
                    FireRangedWeapon(weaponData);
                    yield return new WaitForSeconds(weaponData.cooldown); // Wait for the weapon's cooldown before firing again
                }
            }
        }
    }

    // Fires a single melee weapon
    private void FireMeleeWeapon(WeaponBehavior weapon)
    {
        StartCoroutine(weapon.HandleVisibility()); // Show melee weapon during the attack
    }

    // Fires a single ranged weapon
    private void FireRangedWeapon(WeaponData weaponData)
    {
        GameObject projectile = Instantiate(weaponData.prefab, firePoint.position, Quaternion.identity);
        WeaponBehavior projectileBehavior = projectile.GetComponent<WeaponBehavior>();

        if (projectileBehavior != null)
        {
            projectileBehavior.SetDirection(currentFireDirection);
            projectileBehavior.damageAmount = weaponData.damageAmount;
            projectileBehavior.isRanged = true;
            Destroy(projectile, 5f); // Destroy the projectile after 5 seconds
        }
    }
}

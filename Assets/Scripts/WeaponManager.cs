using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponData> weaponDataList; // List to hold weapon data
    public Transform firePoint; // Where weapons (projectiles, etc.) will spawn from
    private Vector2 currentFireDirection = Vector2.right; // Default fire direction
    private List<WeaponBehavior> activeWeapons = new List<WeaponBehavior>(); // List of currently active weapons

    private void Start()
    {
        // Initialize active weapons from weapon data
        foreach (var weaponData in weaponDataList)
        {
            WeaponBehavior weapon = Instantiate(weaponData.prefab, transform).GetComponent<WeaponBehavior>();
            weapon.damageAmount = weaponData.damageAmount;
            weapon.cooldown = weaponData.cooldown;
            weapon.isRanged = weaponData.isRanged;
            activeWeapons.Add(weapon);
        }

        StartCoroutine(FireWeapons()); // Start the firing loop for active weapons
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
            WeaponBehavior projectile = Instantiate(weapon.gameObject, firePoint.position, Quaternion.identity).GetComponent<WeaponBehavior>();
            projectile.SetDirection(currentFireDirection); // Set direction for the projectile
        }
        else
        {
            // Trigger melee weapon logic (like swinging or proximity checks)
            StartCoroutine(weapon.HandleVisibility()); // Show melee weapon during the attack
        }
    }
}

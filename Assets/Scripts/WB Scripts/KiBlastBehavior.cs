using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiBlastBehavior : WeaponBehavior
{
    public GameObject effectPrefab; // Prefab for the Ki Blast effect
    public LayerMask enemyLayer; // Layer to detect enemies
    private float currentBlastRadius;
    private bool isCooldownActive = false; // Tracks if the weapon is on cooldown

    protected override void Start()
    {
        base.Start();
        currentBlastRadius = weaponData.range; // Initialize blast radius from WeaponData
    }

    protected override void OnLevelUp(int newLevel)
    {
        // Increase the blast radius dynamically
        float radiusIncrease = weaponData.range * 0.125f; // Example: 12.5% increase per level
        currentBlastRadius += radiusIncrease;

        Debug.Log($"{weaponData.weaponName} leveled up to {newLevel}! New blast radius: {currentBlastRadius}");
    }

    public override void Attack(Vector2 direction, float playerSpeed)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (isCooldownActive)
        {
            return;
        }

        StartCoroutine(PerformBlast());
    }

    private IEnumerator PerformBlast()
    {
        isCooldownActive = true;

        // Instantiate the animation effect at the player's position
        GameObject blastEffect = Instantiate(effectPrefab, firePoint.position, Quaternion.identity);
        blastEffect.transform.localScale = Vector3.one * (currentBlastRadius / weaponData.range);

        // Parent the blast effect to the player so it follows their movement
        blastEffect.transform.SetParent(firePoint);

        // Damage enemies every second while the animation is active
        float effectDuration = 3f; // Duration of the animation in seconds
        float damageInterval = 0.5f; // Time interval between each damage application

        float elapsedTime = 0f;
        while (elapsedTime < effectDuration)
        {
            DamageEnemiesInRadius();
            yield return new WaitForSeconds(damageInterval);
            elapsedTime += damageInterval;
        }

        // Destroy the effect after the animation ends
        Destroy(blastEffect);
        yield return new WaitForSeconds(weaponData.cooldown); // Wait for the cooldown
        isCooldownActive = false;
    }

    private void DamageEnemiesInRadius()
    {
        // Detect enemies in the blast radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, currentBlastRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Deal damage to enemies
            DoDamage(enemy);
            Debug.Log($"Hit {enemy.name} with {weaponData.weaponName}!");
        }
    }

}

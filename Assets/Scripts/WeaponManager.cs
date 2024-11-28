using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<WeaponData> weaponDataList; // List of all available weapons
    public Transform firePoint; // Spawn point for ranged projectiles
    private Vector2 currentFireDirection = Vector2.right; // Default fire direction

    private Dictionary<string, int> weaponLevels = new Dictionary<string, int>();
    private Dictionary<string, WeaponBehavior> activeWeapons = new Dictionary<string, WeaponBehavior>();

    private void Start()
    {
        foreach (var weaponData in weaponDataList)
        {
            weaponLevels[weaponData.weaponName] = (weaponData.weaponName == "Shuriken") ? 1 : 0;

            if (weaponData.weaponName == "Shuriken")
            {
                ActivateWeapon(weaponData);
            }
        }

        StartCoroutine(FireWeapons());
        StartCoroutine(FireRangedWeapons());
    }

    private void ActivateWeapon(WeaponData weaponData)
    {
        if (!activeWeapons.ContainsKey(weaponData.weaponName))
        {
            var weapon = Instantiate(weaponData.prefab, transform).GetComponent<WeaponBehavior>();
            weapon.weaponData = weaponData;
            activeWeapons[weaponData.weaponName] = weapon;
        }
    }

    public void UpgradeWeapon(string weaponName)
    {
        if (!weaponLevels.ContainsKey(weaponName)) return;

        int currentLevel = weaponLevels[weaponName];
        if (currentLevel >= 8)
        {
            Debug.Log($"{weaponName} is already at max level!");
            LevelUpManager.Instance?.ShowWeaponUpgradeWarning($"{weaponName} is already at max level!");
            return;
        }

        weaponLevels[weaponName]++;
        int newLevel = weaponLevels[weaponName];
        Debug.Log($"{weaponName} upgraded to Level {newLevel}.");

        if (currentLevel == 0)
        {
            var weaponData = weaponDataList.Find(w => w.weaponName == weaponName);
            if (weaponData != null) ActivateWeapon(weaponData);
        }

        if (activeWeapons.ContainsKey(weaponName))
        {
            activeWeapons[weaponName].LevelUp(newLevel);
        }
    }

    public int GetWeaponLevel(string weaponName)
    {
        return weaponLevels.ContainsKey(weaponName) ? weaponLevels[weaponName] : 0;
    }

    public void SetFireDirection(Vector2 direction)
    {
        currentFireDirection = direction;
    }

    private IEnumerator FireWeapons()
    {
        while (true)
        {
            foreach (var weapon in activeWeapons.Values)
            {
                if (!weapon.weaponData.isRanged && weapon.CanFire())
                {
                    StartCoroutine(weapon.HandleVisibility());
                    weapon.ResetFireTime();
                }
            }
            yield return null;
        }
    }

    private IEnumerator FireRangedWeapons()
    {
        while (true)
        {
            foreach (var weapon in activeWeapons.Values)
            {
                if (weapon.weaponData.isRanged && weapon.CanFire())
                {
                    FireRangedWeapon(weapon.weaponData, weaponLevels[weapon.weaponData.weaponName]);
                    weapon.ResetFireTime();
                }
            }
            yield return null;
        }
    }

    private void FireRangedWeapon(WeaponData weaponData, int level)
    {
        int projectileCount = Mathf.Min(level, 8);
        float angleStep = 360f / projectileCount;
        float startAngle = Vector2.SignedAngle(Vector2.right, currentFireDirection);

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            var projectile = Instantiate(weaponData.prefab, firePoint.position, Quaternion.identity);
            var behavior = projectile.GetComponent<WeaponBehavior>();

            if (behavior != null)
            {
                behavior.SetDirection(direction);
                Destroy(projectile, 5f);
            }
        }
    }
}

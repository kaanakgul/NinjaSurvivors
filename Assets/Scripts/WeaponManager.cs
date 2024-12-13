using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public List<WeaponData> weaponDataList;
    private Dictionary<string, WeaponBehavior> activeWeapons = new Dictionary<string, WeaponBehavior>();
    private Dictionary<string, int> weaponLevels = new Dictionary<string, int>();

    private Vector2 currentFireDirection = Vector2.right;

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

    private void Start()
    {
        InitializeWeapons();

        // Activate the Shuriken at level 1 by default
        var shurikenData = weaponDataList.Find(w => w.weaponName == "Shuriken");
        if (shurikenData != null)
        {
            ActivateWeapon(shurikenData, true); // Activates and sets it to level 1
        }
    }


    private void InitializeWeapons()
    {
        foreach (var weaponData in weaponDataList)
        {
            if (!activeWeapons.ContainsKey(weaponData.weaponName))
            {
                GameObject weaponObject = Instantiate(weaponData.prefab, transform);
                WeaponBehavior weapon = weaponObject.GetComponent<WeaponBehavior>();

                if (weapon != null)
                {
                    weapon.weaponData = weaponData;
                    activeWeapons[weaponData.weaponName] = weapon;

                    // Start inactive for level 0 weapons
                    weapon.gameObject.SetActive(false);
                    weaponLevels[weaponData.weaponName] = 0; // Set initial level to 0
                }
            }
        }
    }



    private void ActivateWeapon(WeaponData weaponData, bool initialize = false)
    {
        if (!activeWeapons.ContainsKey(weaponData.weaponName))
        {
            Debug.LogError($"Weapon '{weaponData.weaponName}' is not initialized in activeWeapons.");
            return;
        }

        var weapon = activeWeapons[weaponData.weaponName];

        // Activate and set level if initializing
        if (initialize)
        {
            weapon.LevelUp(1);
            weapon.gameObject.SetActive(true);
            weaponLevels[weaponData.weaponName] = 1;
        }
    }


    public void FireAllWeapons(float playerSpeed)
    {
        foreach (var weapon in activeWeapons.Values)
        {
            if (weapon.CanFire())
            {
                weapon.Attack(currentFireDirection, playerSpeed);
                weapon.ResetFireTime();
            }
        }
    }

    public void SetFireDirection(Vector2 direction)
    {
        currentFireDirection = direction;
    }

    public int GetUpgradeCost(string weaponName)
    {
        if (!activeWeapons.ContainsKey(weaponName)) return 100;

        int currentLevel = activeWeapons[weaponName].GetCurrentLevel();
        return Mathf.RoundToInt(100 * Mathf.Pow(2, currentLevel));
    }

    public int GetWeaponLevel(string weaponName)
    {
        return weaponLevels.ContainsKey(weaponName) ? weaponLevels[weaponName] : 0;
    }
    public bool UpgradeWeapon(string weaponName)
    {
        // Check if the weapon exists in the activeWeapons dictionary
        if (!activeWeapons.ContainsKey(weaponName))
        {
            Debug.LogError($"Weapon '{weaponName}' not found in activeWeapons.");
            return false;
        }

        /* Retrieve the weapon and its current level
        
        if (!weaponLevels.ContainsKey(weaponName))
        {
            Debug.LogError($"Weapon '{weaponName}' not found in weaponLevels.");
            return false;
        }*/
        var weapon = activeWeapons[weaponName];
        int currentLevel = weaponLevels[weaponName];

        // Activate and upgrade the weapon if it's at level 0
        if (currentLevel == 0)
        {
            ActivateWeapon(weapon.weaponData, true);
            return true;
        }


        // Check if the weapon has reached its max level
        if (currentLevel >= 8)
        {
            Debug.Log($"{weaponName} is already at max level!");
            return false;
        }

        // Calculate the cost of the upgrade
        int cost = GetUpgradeCost(weaponName);

        // Check if the player has sufficient gold
        if (PlayerStats.Instance.gold < cost)
        {
            Debug.Log($"Not enough gold! {weaponName} upgrade costs {cost} gold.");
            return false;
        }

        // Deduct the gold and update the level
        PlayerStats.Instance.gold -= cost;
        weaponLevels[weaponName] = currentLevel + 1;

        // Apply the level-up logic to the weapon
        weapon.LevelUp(weaponLevels[weaponName]);

        // Log the upgrade details
        Debug.Log($"{weaponName} upgraded to Level {weaponLevels[weaponName]}. Cost: {cost} gold. Remaining gold: {PlayerStats.Instance.gold}");

        return true;
    }



}

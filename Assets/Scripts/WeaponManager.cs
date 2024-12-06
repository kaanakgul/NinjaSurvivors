using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public List<WeaponData> weaponDataList;
    private Dictionary<string, WeaponBehavior> activeWeapons = new Dictionary<string, WeaponBehavior>();

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
        foreach (var weaponData in weaponDataList)
        {
            if (weaponData.weaponName == "Shuriken")
            {
                ActivateWeapon(weaponData, true);
            }
        }
    }

    private void ActivateWeapon(WeaponData weaponData, bool initialize = false)
    {
        if (!activeWeapons.ContainsKey(weaponData.weaponName))
        {
            GameObject weaponObject = Instantiate(weaponData.prefab, transform);
            WeaponBehavior weapon = weaponObject.GetComponent<WeaponBehavior>();

            if (weapon != null)
            {
                weapon.weaponData = weaponData;
                activeWeapons[weaponData.weaponName] = weapon;

                if (initialize)
                {
                    weapon.LevelUp(1); // Initialize starting level
                }
                weaponObject.transform.position = new Vector3(-1000f, -1000f, 0f); // Move it far away from the map
            }
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
        if (!activeWeapons.ContainsKey(weaponName)) return 0;

        int currentLevel = activeWeapons[weaponName].GetCurrentLevel();
        return Mathf.RoundToInt(100 * Mathf.Pow(2, currentLevel));
    }

    public int GetWeaponLevel(string weaponName)
    {
        if (!activeWeapons.ContainsKey(weaponName)) return 0;

        return activeWeapons[weaponName].GetCurrentLevel();
    }
    public bool UpgradeWeapon(string weaponName)
{
    if (!activeWeapons.ContainsKey(weaponName)) return false;

    var weapon = activeWeapons[weaponName];
    int currentLevel = weapon.GetCurrentLevel();

    if (currentLevel >= 8)
    {
        Debug.Log($"{weaponName} is already at max level!");
        return false;
    }

    int cost = GetUpgradeCost(weaponName);

    if (PlayerStats.Instance.gold < cost)
    {
        Debug.Log($"Not enough gold! {weaponName} upgrade costs {cost} gold.");
        return false;
    }

    PlayerStats.Instance.gold -= cost;
    weapon.LevelUp(currentLevel + 1);

    Debug.Log($"{weaponName} upgraded to Level {currentLevel + 1}. Cost: {cost} gold. Remaining gold: {PlayerStats.Instance.gold}");
    return true;
}


}

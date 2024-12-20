using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaBehavior : WeaponBehavior
{
    public float attackArcAngle = 90f; // Sweep angle of the katana
    public LayerMask enemyLayer; // Layer for enemy detection
    private int comboSwings = 1; // Number of swings per attack
    private List<float> swingRanges = new List<float>(); // List to hold range for each swing
    private Vector2 lastDirection;
    public GameObject cutEffectPrefab; // Reference to the cut effect prefab


    protected override void Start()
    {
        base.Start();
        InitializeSwingRanges(); // Initialize swing ranges with base range
    }

    private void InitializeSwingRanges()
    {
        swingRanges.Clear();
        for (int i = 0; i < comboSwings; i++)
        {
            swingRanges.Add(weaponData.range); // Set initial range for all swings
        }
    }

    public override void Attack(Vector2 direction, float playerSpeed)
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned. Make sure it is properly initialized.");
            return;
        }

        lastDirection = direction;

        // Perform combo swings
        StartCoroutine(PerformComboSwings());
    }

    private IEnumerator PerformComboSwings()
    {
        for (int i = 0; i < comboSwings; i++)
        {
            Vector2 attackDirection = GetSwingDirection(i);
            PerformSingleSwing(attackDirection, swingRanges[i]);
            yield return new WaitForSeconds(weaponData.cooldown / comboSwings); // Delay between swings
        }
    }

    private void PerformSingleSwing(Vector2 direction, float range)
    {
        // Calculate the swing position and angle
        Vector3 swingPosition = firePoint.position;
        float swingAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Detect enemies within the attack arc
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swingPosition, range, enemyLayer);
        foreach (var enemy in hitEnemies)
        {
            Vector2 toEnemy = enemy.transform.position - swingPosition;
            float angleToEnemy = Vector2.Angle(direction, toEnemy);

            if (angleToEnemy <= attackArcAngle / 2)
            {
                // Deal damage
                DoDamage(enemy);
                Debug.Log($"Hit {enemy.name} with Katana!");
            }
        }

        // Optional: Trigger visual or animation effect here
        Debug.DrawRay(swingPosition, direction * range, Color.red, 0.5f);
    }

    private Vector2 GetSwingDirection(int swingIndex)
    {
        float angleStep = 360f / Mathf.Max(1, comboSwings);
        float angle = angleStep * swingIndex;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        return rotation * lastDirection;
    }

    protected override void OnLevelUp(int newLevel)
    {
        

        if (newLevel % 2 == 1)
        {
            // Add a new swing direction
            comboSwings = Mathf.Min(comboSwings + 1, 8); // Cap swings at 8
            swingRanges.Add(weaponData.range); // Add base range for the new swing
            Debug.Log($"Katana leveled up to odd level {newLevel}! Added a new swing direction.");
        }
        else
        {
            // Increase the range of the last unlocked swing
            int lastSwingIndex = comboSwings - 1;
            if (lastSwingIndex >= 0 && lastSwingIndex < swingRanges.Count)
            {
                swingRanges[lastSwingIndex] *= 1.5f; // Increase range by 150%
                Debug.Log($"Katana leveled up to even level {newLevel}! Increased range of the last swing by 150%.");
            }
        }

        Debug.Log($"Katana leveled up! Combo swings: {comboSwings}, Swing ranges: {string.Join(", ", swingRanges)}");
    }
}

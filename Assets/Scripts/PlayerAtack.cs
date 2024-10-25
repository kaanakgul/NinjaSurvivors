using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public WeaponManager weaponManager;
    public float fireRate = 0.5f; // Fire rate interval
    private Vector2 lastMoveDirection = Vector2.right; // Default direction is right
    private Coroutine fireCoroutine;

    private void Start()
    {
        if (weaponManager == null)
        {
            Debug.LogError("WeaponManager is not assigned in PlayerAttack.");
            return;
        }

        fireCoroutine = StartCoroutine(FireWeaponRoutine());
    }

    private void Update()
    {
        // Update last move direction based on player input or movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
            weaponManager.SetFireDirection(lastMoveDirection); // Pass updated direction to WeaponManager
        }
    }

    private IEnumerator FireWeaponRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            weaponManager.SetFireDirection(lastMoveDirection); // Ensure WeaponManager has the correct direction
        }
    }
}

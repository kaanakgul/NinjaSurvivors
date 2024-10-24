using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public WeaponManager weaponManager; // Reference to the WeaponManager
    private Vector2 lastMoveDirection = Vector2.right; // Default move direction is right

    private void Start()
    {
        if (weaponManager == null)
        {
            weaponManager = GetComponent<WeaponManager>(); // Get the WeaponManager if not assigned
        }
    }

    private void Update()
    {
        // Update last move direction based on player input or movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized; // Update the direction based on movement
        }

        // Update firing direction for ranged weapons
      //  weaponManager.SetFireDirection(lastMoveDirection); // Pass the direction to the WeaponManager
    }
}

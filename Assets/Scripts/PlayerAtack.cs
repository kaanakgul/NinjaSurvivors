using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public WeaponManager weaponManager;
    public float fireRate = 0.5f;
    private Vector2 lastMoveDirection = Vector2.right;

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
            weaponManager.SetFireDirection(lastMoveDirection);
        }

        if (weaponManager != null)
        {
            weaponManager.FireAllWeapons(PlayerStats.Instance.moveSpeed);
        }
    }
}

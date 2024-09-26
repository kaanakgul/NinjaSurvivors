using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    public GameObject shurikenPrefab; // Prefab of the shuriken
    public Transform firePoint; // Where the shuriken will spawn
    public float fireRate = 0.5f; // How often to fire the shuriken

    private Vector2 lastMoveDirection = Vector2.right; // Default direction is right
    private Coroutine fireCoroutine;

    private void Start()
    {
        fireCoroutine = StartCoroutine(FireShurikenRoutine());
    }

    private void Update()
    {
        // Update last move direction based on player input or movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }
    }

    private IEnumerator FireShurikenRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            // Fire the shuriken in the last move direction
            FireShuriken(lastMoveDirection);
        }
    }

    private void FireShuriken(Vector2 direction)
    {
        GameObject shuriken = Instantiate(shurikenPrefab, firePoint.position, Quaternion.identity);

        // Set the direction of the shuriken
        ShurikenBehavior shurikenScript = shuriken.GetComponent<ShurikenBehavior>();
        shurikenScript.SetDirection(direction);
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; } // Singleton instance
    private float speed; // Movement speed derived from PlayerStats
    public float obstacleDistance = 0.5f; // Distance for obstacle detection
    private string currentAnimation; // Current active animation
    private Animator animator;

    private const string UpKey = "WalkUp";
    private const string DownKey = "WalkDown";
    private const string RightKey = "WalkRight";
    private const string LeftKey = "WalkLeft";
    private const string IdleKey = "Idle";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Assign instance
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (PlayerStats.Instance != null)
        {
            speed = PlayerStats.Instance.moveSpeed; // Initialize speed from PlayerStats
        }
    }

    private void Update()
    {
        UpdateSpeedFromStats();
        Move();
    }

    private void LateUpdate()
    {
        Animate();
    }

    private void UpdateSpeedFromStats()
    {
        // Dynamically update speed from PlayerStats
        if (PlayerStats.Instance != null)
        {
            speed = PlayerStats.Instance.moveSpeed;
        }
    }

    private void Move()
    {
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);

        if (right && left) { right = false; left = false; }
        if (up && down) { up = false; down = false; }

        Vector3 movement = Vector3.zero;

        if (up && !IsObstacleAhead(Vector3.up))
        {
            ChangeAnimation(UpKey);
            movement += Vector3.up;
        }
        else if (down && !IsObstacleAhead(Vector3.down))
        {
            ChangeAnimation(DownKey);
            movement += Vector3.down;
        }
        else if (right && !IsObstacleAhead(Vector3.right))
        {
            ChangeAnimation(RightKey);
            movement += Vector3.right;
        }
        else if (left && !IsObstacleAhead(Vector3.left))
        {
            ChangeAnimation(LeftKey);
            movement += Vector3.left;
        }
        else
        {
            ChangeAnimation(IdleKey);
        }

        // Normalize movement if moving diagonally
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Apply movement with speed
        transform.position += movement * speed * Time.deltaTime;
    }

    private void ChangeAnimation(string animationName)
    {
        if (currentAnimation == animationName) return;

        currentAnimation = animationName;
        animator.SetTrigger(currentAnimation);
    }

    private void Animate()
    {
        // Retain this method in case you need further animation updates
    }

    private bool IsObstacleAhead(Vector3 direction)
    {
        Vector3 checkPosition = transform.position + direction * obstacleDistance;

        Collider2D col = Physics2D.OverlapBox(checkPosition, Vector2.one * 0.2f, 0, LayerMask.GetMask("Obstacle"));
        return col != null;
    }
    public Vector2 GetCurrentDirection()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) direction += Vector2.up;
        if (Input.GetKey(KeyCode.S)) direction += Vector2.down;
        if (Input.GetKey(KeyCode.A)) direction += Vector2.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector2.right;

        return direction.normalized;
    }
}

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

        Vector3 movement = Vector3.zero;

        // Add movement based on input and obstacle checks
        if (up && !IsObstacleAhead(Vector3.up))
        {
            movement += Vector3.up;
        }
        if (down && !IsObstacleAhead(Vector3.down))
        {
            movement += Vector3.down;
        }
        if (right && !IsObstacleAhead(Vector3.right))
        {
            movement += Vector3.right;
        }
        if (left && !IsObstacleAhead(Vector3.left))
        {
            movement += Vector3.left;
        }

        // Normalize movement if moving diagonally
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Apply movement with speed
        transform.position += speed * Time.deltaTime * movement;

        // Update animations based on movement
        UpdateAnimations(movement);
    }

    private void UpdateAnimations(Vector3 movement)
    {
        if (movement == Vector3.zero)
        {
            ChangeAnimation(IdleKey);
            return;
        }

        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            // Horizontal movement
            if (movement.x > 0)
            {
                ChangeAnimation(RightKey);
            }
            else
            {
                ChangeAnimation(LeftKey);
            }
        }
        else
        {
            // Vertical movement
            if (movement.y > 0)
            {
                ChangeAnimation(UpKey);
            }
            else
            {
                ChangeAnimation(DownKey);
            }
        }
    }

    private void ChangeAnimation(string animationName)
    {
        if (currentAnimation == animationName) return;

        currentAnimation = animationName;
        animator.SetTrigger(currentAnimation);
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

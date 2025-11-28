using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 inputDirection;
    private Pulpit currentPulpit;
    
    private float speedMultiplier = 1f;
    private bool isInvincible = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        if (GameConfig.Instance != null)
        {
            if (GameConfig.Instance.IsConfigLoaded)
            {
                moveSpeed = GameConfig.Instance.GetPlayerSpeed();
            }
            else
            {
                GameConfig.Instance.OnConfigLoaded += OnConfigLoaded;
            }
        }
    }

    private void OnDestroy()
    {
        if (GameConfig.Instance != null)
        {
            GameConfig.Instance.OnConfigLoaded -= OnConfigLoaded;
        }
    }

    private void OnConfigLoaded()
    {
        moveSpeed = GameConfig.Instance.GetPlayerSpeed();
    }

    private void Update()
    {
        float x = 0f;
        float z = 0f;

        if (GameInput.Instance.IsLeftActionPressed()) x = -1f;
        if (GameInput.Instance.IsRightActionPressed()) x =  1f;
        if (GameInput.Instance.IsUpActionPressed()) z =  1f;
        if (GameInput.Instance.IsDownActionPressed()) z = -1f;

        inputDirection = new Vector3(x, 0f, z).normalized;

        CheckForPulpit();
    }

    private void CheckForPulpit()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            Pulpit pulpit = hit.collider.GetComponent<Pulpit>();
            if (pulpit != null)
            {
                if (currentPulpit != pulpit)
                {
                    currentPulpit = pulpit;
                    if (ScoreManager.Instance != null)
                    {
                        ScoreManager.Instance.IncrementScore();
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 target = rb.position + inputDirection * (moveSpeed * speedMultiplier) * Time.fixedDeltaTime;
        
        if (isInvincible)
        {
            // Prevent falling by clamping Y position if it tries to go below a threshold
            // Or simply rely on the Rigidbody constraint we toggle
            target.y = Mathf.Max(target.y, 0f); // Assuming 0 is platform level
        }
        
        rb.MovePosition(target);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
        if (isInvincible)
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
            // Reset rotation constraints just in case, but keep FreezeRotation
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            
            // Reset velocity to prevent accumulated gravity force when constraint is removed
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        }
        else
        {
            // Remove FreezePositionY
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}

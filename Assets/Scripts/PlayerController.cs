using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;   

    private Rigidbody rb;
    private Vector3 inputDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    private void Start()
    {
        if (GameConfig.Instance != null && GameConfig.Instance.IsConfigLoaded)
        {
            moveSpeed = GameConfig.Instance.GetPlayerSpeed();
        }
        else
        {
            Debug.LogWarning("GameConfig not found, using default speed: " + moveSpeed);
        }
    }
    private void Update()
    {
        float x = 0f;
        float z = 0f;

        if (GameInput.Instance.IsLeftActionPressed()) x = -1f;
        if (GameInput.Instance.IsRightActionPressed()) x = 1f;
        if (GameInput.Instance.IsUpActionPressed()) z = 1f;
        if (GameInput.Instance.IsDownActionPressed()) z = -1f;

        inputDirection = new Vector3(x, 0f, z).normalized;
    }

    private void FixedUpdate()
    {
        Vector3 move = inputDirection * moveSpeed;
        Vector3 velocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        rb.linearVelocity = velocity;
    }
}

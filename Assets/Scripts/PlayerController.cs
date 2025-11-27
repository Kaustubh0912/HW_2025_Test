using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 inputDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        if (GameConfig.Instance != null && GameConfig.Instance.IsConfigLoaded)
        {
            moveSpeed = GameConfig.Instance.GetPlayerSpeed();
        }
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
    }

    private void FixedUpdate()
    {
        Vector3 target = rb.position + inputDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(target);
    }
}

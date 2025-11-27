using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 inputDirection;
    private Pulpit currentPulpit;

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
        Vector3 target = rb.position + inputDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(target);
    }
}

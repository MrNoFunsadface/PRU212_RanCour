using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 defaultSpawnPoint;
    [SerializeField] private float moveSpeed = 1f;

    [Header("Camera Follow Settings")]
    [SerializeField] private float horizontalFollowOffset = 2f;
    [SerializeField] private float verticalFollowOffset = 2f;
    [SerializeField] private float offsetSmoothSpeed = 5f;
    [SerializeField] private bool cameraPanning = true; // Flag to control camera panning
    private Vector3 initialCameraOffset; // Store initial camera offset
    private float currentXOffset = 0f;
    private float currentYOffset = 0f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private CinemachineFollow cinemachineFollow;


    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        
        // Store the initial camera offset
        if (cinemachineFollow != null)
            initialCameraOffset = cinemachineFollow.FollowOffset;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    private void OnDestroy()
    {
        playerControls.Dispose();
    }

    public bool SetSpeed(float speed)
    {
        moveSpeed = speed;
        if (moveSpeed < 0)
        {
            Debug.LogError("Speed cannot be negative. Setting to default value of 1.");
            moveSpeed = 1f;
            return false;
        }
        return true;
    }

    private void Start()
    {
        InitializePlayerPosition();
        InitializeCameraFollow();
    }

    private void InitializePlayerPosition()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.isReturningFromBattle)
            {
                transform.position = GameManager.Instance.playerReturnPosition;
                GameManager.Instance.isReturningFromBattle = false;
            }
            else if (!GameManager.Instance.useCustomSpawnPosition)
            {
                transform.position = defaultSpawnPoint;
            }
        }
        else
        {
            transform.position = defaultSpawnPoint;
        }
    }

    private void InitializeCameraFollow()
    {
        cinemachineFollow = FindFirstObjectByType<CinemachineFollow>();
        if (cinemachineFollow == null)
        {
            Debug.LogWarning("CinemachineFollow not found in the scene. Camera panning will not work.");
            cameraPanning = false; // Disable camera panning if no CinemachineFollow is found
        }
        else
        {
            // IMPORTANT: Store the initial offset AFTER finding the component
            initialCameraOffset = cinemachineFollow.FollowOffset;
        }
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        var stairMover = GetComponent<PlayerStairMovement>();
        if (stairMover != null)
        {
            stairMover.Move(movement, moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
        }
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // Adjust player sprite facing direction
        mySpriteRenderer.flipX = mousePos.x < playerScreenPoint.x;

        if (cinemachineFollow != null) AdjustCameraFollowOffset(mousePos, playerScreenPoint, cameraPanning);
    }

    private void AdjustCameraFollowOffset(Vector2 mousePos, Vector2 playerScreenPoint, bool cameraPanning)
    {
        // Apply different behavior based on whether camera panning is enabled
        if (cameraPanning)
        {
            // Calculate target offsets
            float targetXOffset = mousePos.x < playerScreenPoint.x ? -horizontalFollowOffset : horizontalFollowOffset;
            float targetYOffset = mousePos.y < playerScreenPoint.y ? -verticalFollowOffset : verticalFollowOffset;

            // Smoothly interpolate components
            currentXOffset = Mathf.Lerp(currentXOffset, targetXOffset, Time.deltaTime * offsetSmoothSpeed);
            currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, Time.deltaTime * offsetSmoothSpeed);

            // Snap to target values if very close to reduce tiny oscillations
            currentXOffset = Mathf.Abs(currentXOffset - targetXOffset) > 0.1f ? currentXOffset : targetXOffset;
            currentYOffset = Mathf.Abs(currentYOffset - targetYOffset) > 0.1f ? currentYOffset : targetYOffset;
        }
        else
        {
            // Smoothly return to initial offset when panning is disabled
            currentXOffset = Mathf.Lerp(currentXOffset, 0f, Time.deltaTime * offsetSmoothSpeed);
            currentYOffset = Mathf.Lerp(currentYOffset, 0f, Time.deltaTime * offsetSmoothSpeed);

            // Snap to zero if very close to reduce tiny oscillations
            currentXOffset = Mathf.Abs(currentXOffset) < 0.1f ? 0f : currentXOffset;
            currentYOffset = Mathf.Abs(currentYOffset) < 0.1f ? 0f : currentYOffset;
        }

        // Apply the calculated offsets while preserving Z component
        Vector3 newOffset = initialCameraOffset;
        newOffset.x = initialCameraOffset.x + currentXOffset;
        newOffset.y = initialCameraOffset.y + currentYOffset;
        cinemachineFollow.FollowOffset = newOffset;
    }

    public void SetCameraPanning(bool enable)
    {
        cameraPanning = enable;
        if (cinemachineFollow != null)
        {
            // Reset the camera offset to the initial value when toggling panning
            cinemachineFollow.FollowOffset = initialCameraOffset;
            currentXOffset = 0f; // Reset current X offset
        }
    }
}

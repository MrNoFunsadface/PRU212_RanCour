﻿using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 defaultSpawnPoint;
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
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

    public bool setSpeed(float speed)
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
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
        }
    }

}

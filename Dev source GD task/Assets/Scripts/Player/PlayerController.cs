using System.Collections;
using UnityEngine;

public class PlayerController :MonoBehaviour {
    public static PlayerController Instance { private set; get; }
    private Rigidbody2D playerRigidbody;

    [SerializeField] private int currentHealth = 100;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed = 15f;
    private float currentSpeed;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    private enum PlayerState
    {
        Idle,
        Running,
        Jumping
    }
    private PlayerState playerState;

    private void Awake()
    {
        Instance = this;
        playerRigidbody = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
    }
    private void Start()
    {
        InputManager.Instance.OnSprintButtonPressed += SprintStart;
        InputManager.Instance.OnSprintButtonReleased += SprintStop;

        InputManager.Instance.OnJumpButtonPressed += () => StartCoroutine(Jump());
    }

    #region Sprinting
    private void SprintStart()
    {
        currentSpeed = runSpeed;
        playerState = PlayerState.Running;
    }
    private void SprintStop()
    {
        currentSpeed = moveSpeed;
        playerState = PlayerState.Idle;
    }
    #endregion

    #region Jumping
    IEnumerator Jump()
    {
        float cayoteTime = 0.1f;
        float elapsedTime = 0f;
        while (elapsedTime < cayoteTime)
        {
            if (isGrounded)
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpSpeed);
                playerState = PlayerState.Jumping;
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    #endregion
    private void FixedUpdate()
    {
        if (GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Playing) return;

        HandleMovement();
        CheckGround();
    }
    private void HandleMovement()
    {
        float moveInput = InputManager.Instance.GetMovementInput() * currentSpeed;

        playerRigidbody.velocity = new Vector2(moveInput, playerRigidbody.velocity.y);

        HandleFilipping(moveInput);
    }
    private void HandleFilipping(float moveInput)
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void CheckGround()
    {
        bool isGround = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        if (isGround)
        {
            isGrounded = true;
            if (playerState == PlayerState.Jumping)
            {
                playerState = PlayerState.Idle;
            }
        }
        else
        {
            isGrounded = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver(GameManager.GameOverType.Died);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }
    private IEnumerator FlashRed()
    {
        Color bodyColor = bodySpriteRenderer.color;
        if (bodySpriteRenderer)
        {
            bodySpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            bodySpriteRenderer.color = bodyColor;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FinishLine>() != null)
        {
            GameManager.Instance.GameOver(GameManager.GameOverType.Victory);
        }
        else if (collision.GetComponent<DamageArea>() != null)
        {
            GameManager.Instance.GameOver(GameManager.GameOverType.Died);
        }
        else if (collision.GetComponent<Coin>() != null)
        {
            Coin coin = collision.GetComponent<Coin>();
            GameManager.Instance.CollectCoin(coin.GetCoinValue());
            coin.DestroyCoin();
        }
    }
}

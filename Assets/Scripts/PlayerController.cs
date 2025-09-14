using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class InputConfig
    {
        public KeyCode up = KeyCode.W;
        public KeyCode down = KeyCode.S;
        public KeyCode left = KeyCode.A;
        public KeyCode right = KeyCode.D;
        public KeyCode bomb = KeyCode.Space;
    }

    [Header("Input Configuration")]
    public InputConfig inputConfig;

    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Sprite Renderers")]
    [SerializeField] private AnimatedSpriteRenderer spriteRendererUp;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererDown;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererLeft;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererRight;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererDeath;

    [Header("Components")]
    [SerializeField] private BombController bombController;

    public Rigidbody2D rb { get; private set; }

    private AnimatedSpriteRenderer activeSpriteRenderer;
    private Vector2 direction = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetActiveSpriteRenderer(spriteRendererDown);
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Debug.Log("Player Hit by Explosion");
            DeathSequence();
        }
    }

    private void HandleInput()
    {
        bool moved = false;

        if (Input.GetKey(inputConfig.up))
        {
            SetDirection(Vector2.up, spriteRendererUp);
            moved = true;
        }
        else if (Input.GetKey(inputConfig.down))
        {
            SetDirection(Vector2.down, spriteRendererDown);
            moved = true;
        }
        else if (Input.GetKey(inputConfig.left))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
            moved = true;
        }
        else if (Input.GetKey(inputConfig.right))
        {
            SetDirection(Vector2.right, spriteRendererRight);
            moved = true;
        }

        if (!moved)
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }

        if (Input.GetKeyDown(inputConfig.bomb))
        {
            if (bombController != null)
            {
                bombController.TryPlaceBomb();
            }
        }
    }

    private void Move()
    {
        Vector2 position = rb.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        SetActiveSpriteRenderer(spriteRenderer);
        direction = newDirection;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    private void SetActiveSpriteRenderer(AnimatedSpriteRenderer renderer)
    {
        spriteRendererUp.enabled = renderer == spriteRendererUp;
        spriteRendererDown.enabled = renderer == spriteRendererDown;
        spriteRendererLeft.enabled = renderer == spriteRendererLeft;
        spriteRendererRight.enabled = renderer == spriteRendererRight;
        activeSpriteRenderer = renderer;
    }

    private void DeathSequence()
    {
        enabled = false;
        SetAllSpriteRenderers(false);
        spriteRendererDeath.enabled = true;
        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void SetAllSpriteRenderers(bool state)
    {
        spriteRendererDown.enabled = state;
        spriteRendererUp.enabled = state;
        spriteRendererLeft.enabled = state;
        spriteRendererRight.enabled = state;
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinState();
    }

}

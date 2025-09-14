using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float animationTime = 0.25f;
    [SerializeField] private bool loop = true;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    public bool idle = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }

    private void NextFrame()
    {
        animationFrame++;


        if (loop && animationFrame >= animationFrames.Length)
        {
            animationFrame = 0;
        }

        // Mevcut duruma göre sprite'ı güncelle
        if (idle)
        {
            spriteRenderer.sprite = idleSprite;
        }
        else if (animationFrame >= 0 && animationFrame < animationFrames.Length)
        {
            spriteRenderer.sprite = animationFrames[animationFrame];
        }
    }


}

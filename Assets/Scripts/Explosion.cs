using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Sprites")]
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;

    public void SetActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    /// Patlamanın dönüşünü yöne göre ayarlar
    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void DestroyAfterSeconds(float seconds)
    {
        Destroy(gameObject, seconds);
    }

}
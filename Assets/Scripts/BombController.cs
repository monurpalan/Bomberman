using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombFuseTime = 3f;
    [SerializeField] private int bombAmount = 1;
    [SerializeField] private float offset = 0.5f;

    [Header("Explosion Settings")]
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] private float explosionDuration = 1f;
    [SerializeField] public int explosionRadius = 1;
    [SerializeField] private LayerMask explosionLayerMask;

    [Header("Destructible Settings")]
    [SerializeField] private Destructible destructiblePrefab;
    [SerializeField] private Tilemap destructibleTiles;

    private int bombsRemaining;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Oyuncu bir bombanın üstünden ayrıldığında, bombanın collider’ı artık fiziksel engel olarak çalışır
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
            Debug.Log("Bomb Trigger Exit");
        }
    }

    public void TryPlaceBomb()
    {
        if (bombsRemaining > 0)
        {
            StartCoroutine(PlaceBomb());
        }
    }

    public void IncreaseBombCount()
    {
        bombAmount++;
        bombsRemaining++;
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = GetGridAlignedPosition(transform.position);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = GetGridAlignedPosition(bomb.transform.position);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        Destroy(explosion.gameObject, explosionDuration);
        explosion.DestroyAfterSeconds(explosionDuration);

        // Tüm yönlerde patlama oluştur
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    /// Pozisyonu grid koordinatlarına hizalar
    private Vector2 GetGridAlignedPosition(Vector2 originalPosition)
    {
        return new Vector2(
            Mathf.Floor(originalPosition.x) + offset,
            Mathf.Floor(originalPosition.y) + offset
        );
    }

    /// Belirli bir yönde patlama zinciri oluşturur
    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (length <= 0) return;

            position += direction;

            // Engelleri kontrol et
            if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
            {
                ClearDestructible(position);
                return;
            }

            // Patlama segmenti oluştur
            Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
            explosion.SetDirection(direction);
            Destroy(explosion.gameObject, explosionDuration);
            explosion.DestroyAfterSeconds(explosionDuration);

            // Patlama zincirini devam ettir
            Explode(position, direction, length - 1);
        }
    }

    /// Yıkılabilir karoları temizler ve yıkılabilir nesneler oluşturur
    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }


}

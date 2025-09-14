using UnityEngine;

public class Destructible : MonoBehaviour
{


    [Header("Destruction Settings")]
    [SerializeField] private float destructionTime = 1f;

    [Header("Item Spawn Settings")]
    [SerializeField] private float itemSpawnChance = 0.1f;
    [SerializeField] private GameObject[] items;

    private void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        TrySpawnItem();
    }

    private void TrySpawnItem()
    {
        // Eşyaların mevcut olup olmadığını kontrol et
        if (items.Length == 0) return;

        if (Random.value >= itemSpawnChance) return;

        // Rastgele eşya çıkar
        int randomIndex = Random.Range(0, items.Length);
        Instantiate(items[randomIndex], transform.position, Quaternion.identity);
    }


}
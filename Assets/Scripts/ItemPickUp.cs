using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease
    }

    [Header("Item Settings")]
    [SerializeField] private ItemType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Item Picked Up");
        ApplyEffectToPlayer(other.gameObject);
        Destroy(gameObject);
    }

    private void ApplyEffectToPlayer(GameObject player)
    {
        var bombController = player.GetComponent<BombController>();
        var playerController = player.GetComponent<PlayerController>();

        switch (type)
        {
            case ItemType.ExtraBomb:
                bombController?.IncreaseBombCount();
                break;

            case ItemType.BlastRadius:
                if (bombController != null)
                    bombController.explosionRadius++;
                break;

            case ItemType.SpeedIncrease:
                if (playerController != null)
                    playerController.speed++;
                break;
        }
    }

}
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private bool powerUp;
    [SerializeField] private string powerUpName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            if (powerUp)
            {
                AudioManager.Instance.PlaySFX("PowerUpCollect");
                var player = other.GetComponent<Player>();
                UpgradePlayerSkill(player);
            }

            Destroy(gameObject);
        }
    }
    private void UpgradePlayerSkill(Player player)
    {
        switch (powerUpName)
        {
            case "Dash":
                player.dashUpgraded = true;
                break;
            default:
                Debug.LogWarning("Power up não reconhecido!");
                break;
        }
    }
}

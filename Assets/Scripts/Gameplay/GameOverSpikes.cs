using UnityEngine;

public class GameOverSpikes : MonoBehaviour
{
    [SerializeField] private PlayerDeathManager deathManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            deathManager.RespawnAtLastCheckpoint();
        }
    }
}

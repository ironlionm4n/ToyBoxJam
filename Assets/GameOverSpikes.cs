using UnityEngine;

public class GameOverSpikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.IsDead = true;
        }
    }
}

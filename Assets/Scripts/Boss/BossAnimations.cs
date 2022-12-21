using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimations : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animation idle;
    [SerializeField] private Animation attack;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX= true;
        }
        else if (player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject player;

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x >= -5 && player.transform.position.x <= 6)
        {
            transform.position= new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, -10);
        }
        else if(player.transform.position.x < -5)
        {
            transform.position = new Vector3(-5, player.transform.position.y + 1.5f, -10);
        }
        else if (player.transform.position.x > 6)
        {
            transform.position = new Vector3(6, player.transform.position.y + 1.5f, -10);
        }
    }
}

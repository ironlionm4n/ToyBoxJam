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

        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, -10.9f, 11.7f), 
            Mathf.Clamp(player.transform.position.y, 1.5f, 5f), -10);
       
    }
}

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

        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, -5, 6), 
            Mathf.Clamp(player.transform.position.y, 1.5f, 5f), -10);
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private float minX = -16.9f;
    [SerializeField] private float maxX = 17.5f;
    [SerializeField] private float minY = 1.5f;
    [SerializeField] private float maxY = 5f;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, minX, maxX), 
            Mathf.Clamp(player.transform.position.y, minY, maxY), -10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");


    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction from the beam's origin to the player
        Vector3 targetDirection = player.transform.position - transform.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        // Rotate the beam's sprite to face the player
        transform.rotation = Quaternion.AngleAxis(angle -90, Vector3.forward);


    }
}

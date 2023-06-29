using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    bool moving = false;
    bool rightWards = false;
    float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (rightWards)
            {
                transform.position += transform.right * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position -= transform.right * moveSpeed * Time.deltaTime;
            }
        }
    }


    /// <summary>
    /// Takes in the height, spawn position, direction of movement (as bool), and speed of the individual wave.
    /// </summary>
    /// <param name="height"></param>
    /// <param name="spawnPoint"></param>
    /// <param name="rightMoving"></param>
    /// <param name="speed"></param>
    public void Spawn(float height, Vector2 spawnPoint, bool rightMoving, float speed, WaveAttackFalling spawner)
    {
        float newY = ((height - 1) * 0.5f) + -2.5f;

        transform.localScale = new Vector2(transform.localScale.x, height);

        transform.position = new Vector2(spawnPoint.x, newY);

        moveSpeed = speed; 

        rightWards = rightMoving;

        moving = true;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Wall")){
            Destroy(gameObject);
        }
    }
}

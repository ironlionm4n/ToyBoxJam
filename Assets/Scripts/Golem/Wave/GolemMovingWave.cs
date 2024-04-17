using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemMovingWave : MonoBehaviour
{
    [SerializeField]
    private float waveSpeed = 8f;

    [SerializeField]
    private float maxMoveSpeed = 10f;

    private bool move = false;
    private bool moveLeft = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!move)
        {
            return;
        }


        if(!moveLeft)
        {
            transform.position += transform.right * waveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position -= transform.right * waveSpeed * Time.deltaTime;
        }
    }

    public void StartMoving(bool moveLeft)
    {
        this.moveLeft = moveLeft;
        move = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

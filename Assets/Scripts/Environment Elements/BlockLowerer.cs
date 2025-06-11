using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLowerer : MonoBehaviour
{
    [SerializeField] private Lowerable ConnectedLowerable;

    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange)
        {
            if (Input.GetKey(KeyCode.F))
            {
                ConnectedLowerable.SetLowering(true);
            }
            else
            {
                ConnectedLowerable.SetLowering(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            ConnectedLowerable.SetLowering(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCJump : MonoBehaviour
{
    private Transform player;

    private NPCBrain brain;

    private bool jumping = false;

    private void Awake()
    {
        brain = GetComponent<NPCBrain>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = brain.Player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

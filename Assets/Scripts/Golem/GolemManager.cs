using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemManager : MonoBehaviour
{
    private Transform player;

    public Transform Player { get { return player; } }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

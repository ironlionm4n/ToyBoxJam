using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBallAttack : MonoBehaviour
{
    public GameObject SkeletonBallPrefab;

    private GameObject currentBall;

    private bool attacking = false;

    private float ballSpawnTime = 4f;

    private bool currentlySpawning = false;



    // Start is called before the first frame update
    void Start()
    {
        attacking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!attacking) { return; }

        if(currentBall == null && !currentlySpawning)
        {
            currentlySpawning =true;

            StartCoroutine(SpawnNextBall());
        }
    }

    IEnumerator SpawnNextBall()
    {
        // play an animation

        yield return new WaitForSeconds(ballSpawnTime);

        currentBall = Instantiate(SkeletonBallPrefab, transform.position, Quaternion.identity);

        currentlySpawning = false;
    }
}

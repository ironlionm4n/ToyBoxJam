using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawnAttack : MonoBehaviour
{

    public GameObject SkeletonPrefab;

    private bool attacking = false;

    private int maxSkeletons = 3;
    private int numSkeletonsActive = 0;

    private float spawnSkeletonTime = 1.5f;
    private bool spawning = false;

    [SerializeField] private Transform[] SkeletonSpawns;
    public float SkeletonSpawnPreventionRadius = 4f;
    public Transform player;


    // Start is called before the first frame update
    void Start()
    {
        attacking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(attacking && numSkeletonsActive < maxSkeletons)
        {
            if(!spawning)
            {
                spawning = true;
                StartCoroutine(SpawnSkeleton());
            }
        }

        // testing only
        foreach(Transform t in SkeletonSpawns)
        {
            if(Vector2.Distance(t.position, player.transform.position) < SkeletonSpawnPreventionRadius)
            {
                t.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                t.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }

    public void SkeletonDied()
    {
        numSkeletonsActive--;

        if (numSkeletonsActive < 0)
        {
            numSkeletonsActive = 0;
        }
    }

    private IEnumerator SpawnSkeleton()
    {



        int random = Random.Range(0, SkeletonSpawns.Length);

        while (Vector2.Distance(SkeletonSpawns[random].transform.position, player.transform.position) < SkeletonSpawnPreventionRadius)
        {
            random = Random.Range(0, SkeletonSpawns.Length);
        }


        SkeletonSpawns[random].GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(spawnSkeletonTime);

        Instantiate(SkeletonPrefab, SkeletonSpawns[random].transform.position, Quaternion.identity)
            .GetComponent<Skeleton>().SetAttackManager(this);
        numSkeletonsActive++;

        yield return new WaitForSeconds(spawnSkeletonTime);

        spawning = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemDebrisSpawning : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField]
    private Transform DebrisSpawnsHolder;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject Debris;

    [SerializeField]
    private GameObject FallingCoin;

    [Header("Spawning Variables")]
    [SerializeField]
    private float _debrisSpawnTime = 9f;

    [SerializeField]
    private int _coinsPerDebrisSpawn = 1;

    [SerializeField]
    private int _debrisToSpawn = 10;

    private List<Transform> debrisSpawnPoints = new List<Transform>();
    private List<System.Func<IEnumerator>> patterns = new List<System.Func<IEnumerator>>();

    int numDebrisSpawned = 0;
    int numCoinsSpawned = 0;
    bool patternRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in DebrisSpawnsHolder)
        {
            debrisSpawnPoints.Add(child);
        }

        patterns.Add(LeftRightPattern);
        patterns.Add(RightLeftPattern);
        patterns.Add(MiddleOutPattern);
        patterns.Add(OutMiddlePattern);
    }

    public void StartDebris()
    {
        numDebrisSpawned = 0;
        numCoinsSpawned = 0;

        int randomSelection1 = Random.Range(0, 4);

        int randomSelection2 = Random.Range(0, 4);

        // make sure we don't do the same pattern twice in a row
        while(randomSelection2 == randomSelection1)
        {
            randomSelection2 = Random.Range(0, 4);
        }


        StartCoroutine(StartPatterns(patterns[randomSelection1], patterns[randomSelection2]));
    }

    private IEnumerator StartPatterns(System.Func<IEnumerator> _f1, System.Func<IEnumerator> _f2)
    {
        patternRunning = true;
        StartCoroutine(_f1());

        yield return new WaitWhile(() => patternRunning);

        StartCoroutine(_f2());
    }

    private IEnumerator LeftRightPattern()
    {
        for(int i = 0; i < _debrisToSpawn/2; i++)
        {
            DropItem(i);
            yield return new WaitForSeconds(_debrisSpawnTime / debrisSpawnPoints.Count);
        }

        patternRunning = false;
    }

    private IEnumerator RightLeftPattern()
    {
        for (int i = _debrisToSpawn/2; i > 0; i--)
        {
            DropItem(i);
            yield return new WaitForSeconds(_debrisSpawnTime / debrisSpawnPoints.Count);
        }

        patternRunning = false;
    }

    private IEnumerator MiddleOutPattern()
    {
        int midPoint = debrisSpawnPoints.Count / 2;
        for (int i = _debrisToSpawn / 4; i > 0; i--)
        {
            DropItem(midPoint+i);
            DropItem(midPoint-i);
            yield return new WaitForSeconds(_debrisSpawnTime / debrisSpawnPoints.Count);
        }

        patternRunning = false;
    }

    private IEnumerator OutMiddlePattern()
    {
        for (int i = 0; i < _debrisToSpawn / 4; i++)
        {
            DropItem(i);
            DropItem(debrisSpawnPoints.Count - 1 - i);
            
            yield return new WaitForSeconds(_debrisSpawnTime / debrisSpawnPoints.Count);
        }

                patternRunning = false;
    }

    private void DropItem(int _spawnPointIndex)
    {
        // if we haven't spawned all our coins yet and we're almost done, spawn them
        if(numDebrisSpawned == _debrisToSpawn - _coinsPerDebrisSpawn && numCoinsSpawned != _coinsPerDebrisSpawn)
        {
            SpawnCoin(_spawnPointIndex);
            return;
        }

        // if we've spawned all the coins already
        if(numCoinsSpawned == _coinsPerDebrisSpawn)
        {
            SpawnDebris(_spawnPointIndex);
            return;
        }

        int coinSpawnChance = 1;
        int randValue = Random.Range(1, 11);

        // check if we are going to spawn a coin instead of debris
        if(coinSpawnChance/randValue != 0)
        {
            SpawnCoin(_spawnPointIndex);
            return;
        }

        SpawnDebris(_spawnPointIndex);

    }

    private void SpawnDebris(int _spawnPointIndex)
    {
        Instantiate(Debris, debrisSpawnPoints[_spawnPointIndex].position, Quaternion.identity);
        numDebrisSpawned++;
    }

    private void SpawnCoin(int _spawnPointIndex)
    {
        Instantiate(FallingCoin, debrisSpawnPoints[_spawnPointIndex].position, Quaternion.identity);
        numCoinsSpawned++;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinSpawning : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] public GameObject coin;

    private GameObject lastSpawnedCoin;

    public static CoinSpawning instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = new CoinSpawning();
        }
        else
        {
            Debug.LogWarning("More than one coin spawner. Deleting " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public void SpawnCoin(Vector2 spawnLocation)
    {
       lastSpawnedCoin = Instantiate(coin, spawnLocation, Quaternion.identity);
    }

    public void DeleteLastSpawned()
    {
        if(lastSpawnedCoin != null)
        {
            Destroy(lastSpawnedCoin);
        }
    }
}

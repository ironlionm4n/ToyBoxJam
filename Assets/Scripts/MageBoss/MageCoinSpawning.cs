using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCoinSpawning : MonoBehaviour
{
    //Coin spawn points
    [SerializeField] private GameObject[] coinSpawnPointsPhase1;
    [SerializeField] private GameObject centerCoinSpawnPoint;
    [SerializeField] private float _delayBetweenCoinSpawns = 1.4f;

    //Reference to player
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private MageController mController;

    [SerializeField] private GameObject coin;

    private GameObject _currentCoin;

    private bool _spawnCoins = true;
    private bool _coinSpawned = false;
    private bool _spawnedLeft = false;

    private int _currentPhase = 1;

    private void OnEnable()
    {
        mController.phaseChange += PhaseChange;
        playerStats.pickedUpCoin += CoinPickedUp;
    }

    private void OnDisable()
    {
        mController.phaseChange -= PhaseChange;
        playerStats.pickedUpCoin -= CoinPickedUp;
    }

    private void Start()
    {
        if (_spawnCoins && !_coinSpawned)
        {
            _coinSpawned = true;

            SpawnCoin();
        }
    }


    // Update is called once per frame
    void Update()
    {
      
    }

    private void PhaseChange(int phase, float timeBetweenPhases)
    {

        if( _currentCoin != null )
        {
            Destroy(_currentCoin);
        }

        if(phase == 4)
        {
            return;
        }

        _currentPhase = phase;

        //StartCoroutine(WaitForPhaseChange(timeBetweenPhases));
    }

    IEnumerator WaitForPhaseChange(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnCoin();
    }

    private void CoinPickedUp()
    {
        _coinSpawned = false;

        Destroy(_currentCoin );

        SpawnCoin();
    }

    public void SpawnCoin()
    {
        if(_currentPhase == 1 || _currentPhase == 2)
        {
            if (_spawnedLeft)
            {
                _spawnedLeft = false;

                _currentCoin = Instantiate(coin, coinSpawnPointsPhase1[0].transform.position, Quaternion.identity);
            }
            else
            {
                _spawnedLeft = true;

                _currentCoin = Instantiate(coin, coinSpawnPointsPhase1[1].transform.position, Quaternion.identity);
            }
        }
        else
        {
            StartCoroutine(WaitForCoinSpawn());
        }
    }

    /// <summary>
    /// Waits a certain amount of time before respawning the coin
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForCoinSpawn()
    {
        yield return new WaitForSeconds(_delayBetweenCoinSpawns);

        _currentCoin = Instantiate(coin, centerCoinSpawnPoint.transform.position, Quaternion.identity);
    }
}

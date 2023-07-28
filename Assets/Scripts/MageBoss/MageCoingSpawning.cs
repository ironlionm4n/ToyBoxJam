using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCoinSpawning : MonoBehaviour
{
    [SerializeField] private GameObject[] coinSpawnPointsPhase1;
    [SerializeField] private GameObject[] coinsSpawnPointsPhase2;
    [SerializeField] private GameObject[] coinSpawnPointsPhase3;

    private GameObject _currentCoin;

    private bool _spawnCoins = false;
    private bool _coinSapwned = false;

    private int _currentPhase = 1;

    private void OnEnable()
    {
        GetComponent<MageController>().phaseChange += PhaseChange;
    }

    private void OnDisable()
    {
        GetComponent<MageController>().phaseChange -= PhaseChange;
    }

    // Update is called once per frame
    void Update()
    {
      if(_spawnCoins && !_coinSapwned)
        {
            _coinSapwned=true;

        }   
    }

    private void PhaseChange(int phase, float timeBetweenPhases)
    {

        _spawnCoins = false;

        if(phase == 4)
        {
            Destroy(_currentCoin);
            return;
        }

        _currentPhase = phase;

        StartCoroutine(WaitForPhaseChange(timeBetweenPhases));
    }

    IEnumerator WaitForPhaseChange(float time)
    {
        yield return new WaitForSeconds(time);

        _spawnCoins=true;
    }

    private void CoinPickedUp()
    {
        _coinSapwned = false;
    }
}

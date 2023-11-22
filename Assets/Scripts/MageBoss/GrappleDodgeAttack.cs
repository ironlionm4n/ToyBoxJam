using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleDodgeAttack : MonoBehaviour, IAttack
{
    [SerializeField] private GameObject oneWays;
    [SerializeField] private GameObject grappleHookAttack;

    [SerializeField] private MovingGrappleHook[] movingGrappleHooks;
    [SerializeField] private GameObject[] grappleStartingPoints;
    [SerializeField] private Transform[] hookOffScreenPositions;

    [SerializeField] private float grappleMoveDownSpeed = 1f;

    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject projectileSpawnPoint;

    [SerializeField] private MageCoinSpawning coinSpawning;

    List<GameObject> currentProjectiles;

    private int numberOfProjectiles = 15;

    private bool active;
    private bool stopped = false;
    private void OnEnable()
    {
        GetComponent<MageController>().sideBouncingAttack += Attack;
        currentProjectiles = new List<GameObject>();
        currentProjectiles = new List<GameObject>();
    }

    private void OnDisable()
    {
        GetComponent<MageController>().sideBouncingAttack -= Attack;
    }

    public void Attack(IAction action)
    {

        if(stopped) return;

        oneWays.SetActive(false);
        grappleHookAttack.SetActive(true);

        float grappleMoveTime = 2f;

        //Start moving grapples down
        movingGrappleHooks[0].transform.DOMove(grappleStartingPoints[0].transform.position, grappleMoveTime).SetEase(Ease.Linear);
        movingGrappleHooks[1].transform.DOMove(grappleStartingPoints[1].transform.position, grappleMoveTime).SetEase(Ease.Linear);

        active = true;

        StartCoroutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        float timeBetweenProjectiles = 0.9f;

        coinSpawning.SpawnCoin();

        for(int i = 0; i < numberOfProjectiles; i++)
        {
            currentProjectiles.Add(Instantiate(projectile, projectileSpawnPoint.transform.position, Quaternion.identity));

            yield return new WaitForSeconds(timeBetweenProjectiles);

        }
    }

    public void StopAttack()
    {
        stopped = true;

        numberOfProjectiles = 0;

        StopCoroutine(StartAttack());

        foreach(var projectile in currentProjectiles)
        {
            Destroy(projectile.gameObject);
        }

       GameObject.Find("Player").GetComponent<Grapple>().BreakHook();

        DOTween.Kill(movingGrappleHooks[0].transform);
        DOTween.Kill(movingGrappleHooks[1].transform);

        movingGrappleHooks[0].tag = "Untagged";
        movingGrappleHooks[1].tag = "Untagged";

        movingGrappleHooks[0].transform.DOMove(hookOffScreenPositions[0].transform.position, 2f).SetEase(Ease.Linear);
        movingGrappleHooks[1].transform.DOMove(hookOffScreenPositions[1].transform.position, 2f).SetEase(Ease.Linear);

        active = false;
    }

    public bool GetIsActive()
    {
        return active;
    }
}

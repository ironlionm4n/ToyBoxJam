using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleDodgeAttack : MonoBehaviour, IAttack
{
    [SerializeField] private GameObject oneWays;

    [SerializeField] private MovingGrappleHook[] movingGrappleHooks;
    [SerializeField] private GameObject[] grappleStartingPoints;

    [SerializeField] private float grappleMoveDownSpeed = 1f;

    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject projectileSpawnPoint;

    List<GameObject> currentProjectiles;

    private Transform[] hookOffScreenPositions;

    private void OnEnable()
    {
        GetComponent<MageController>().sideBouncingAttack += Attack;
        currentProjectiles = new List<GameObject>();
        hookOffScreenPositions = new Transform[2];

        hookOffScreenPositions[0] = movingGrappleHooks[0].transform;
        hookOffScreenPositions[1] = movingGrappleHooks[1].transform;
    }

    private void OnDisable()
    {
        GetComponent<MageController>().sideBouncingAttack -= Attack;
    }

    public void Attack(IAction action)
    {


        oneWays.SetActive(false);
        float grappleMoveTime = 2f;

        //Start moving grapples down
        movingGrappleHooks[0].transform.DOMove(grappleStartingPoints[0].transform.position, grappleMoveTime).SetEase(Ease.Linear);
        movingGrappleHooks[1].transform.DOMove(grappleStartingPoints[1].transform.position, grappleMoveTime).SetEase(Ease.Linear);

        StartCoroutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        float timeBetweenProjectiles = 0.9f;

        for(int i = 0; i < 15; i++)
        {
            currentProjectiles.Add(Instantiate(projectile, projectileSpawnPoint.transform.position, Quaternion.identity));

            yield return new WaitForSeconds(timeBetweenProjectiles);

        }
    }

    public void StopAttack()
    {
        foreach(var projectile in currentProjectiles)
        {
            Destroy(projectile.gameObject);
        }
    }
}

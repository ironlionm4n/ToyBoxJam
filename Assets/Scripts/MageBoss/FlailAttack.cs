using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlailAttack : MonoBehaviour
{

    public Transform mage { get; private set; }

    public GameObject projectile { get; private set; }

    public int numberOfBackForths { get; private set; }

    public Transform[] movePoints { get; private set; }

    public bool moveRight { get; private set; }

    public bool moving { get; private set; }

    public bool executing { get; private set; }

    public float moveTime { get; private set; }

    public float shootCooldown { get; private set; }

    public List<GameObject> currentProjectiles { get; private set; }

    private void OnEnable()
    {
        GetComponent<MageController>().flailAttack += StartFlailing;
        currentProjectiles = new List<GameObject>();
    }

    private void OnDisable()
    {
        GetComponent<MageController>().flailAttack -= StartFlailing;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFlailing(MageFlailAction action)
    {
        this.mage = action.mage;
        this.projectile = action.projectile;
        this.numberOfBackForths = action.numberOfBackForths;
        this.movePoints = action.movePoints;
        this.moveRight = action.moveRight;
        this.moveTime = action.moveTime;
        this.shootCooldown = action.shootCooldown;

        StartCoroutine(StartMoving());
    }

    public IEnumerator StartMoving()
    {
        //Calculates total distance from starting to end position
        var fullDistance = Vector2.Distance(movePoints[0].position, movePoints[1].position);

        //Calculate velocity of object given the full distance and move time
        var velocity = fullDistance / moveTime;


        for (int i = 0; i < numberOfBackForths; i++)
        {
            moving = true;

            if (moveRight)
            {

                //Calculates how much further the object needs to move
                var remainingDistance = Vector2.Distance(transform.position, movePoints[1].position);

                //Calculate move time
                var time = remainingDistance / velocity;

                mage.DOMove(movePoints[1].position, time).SetEase(Ease.Linear).OnComplete(() => { 
                    moving = false; 
                    moveRight = false; 
                    Shoot(); 
                });
            }
            else
            {

                //Calculates how much further the object needs to move
                var remainingDistance = Vector2.Distance(transform.position, movePoints[0].position);

                //Calculate move time
                var time = remainingDistance / velocity;

                mage.DOMove(movePoints[0].position, time).SetEase(Ease.Linear).OnComplete(() => { 
                    moving = false; 
                    moveRight = true;
                    Shoot(); 
                });
            }

            yield return new WaitWhile(() => moving);
        }

        var distanceToMidPoint = fullDistance / 2;

        Vector2 targetLocation = transform.position.x < 0 ? new Vector2(transform.position.x + distanceToMidPoint, transform.position.y) :
            new Vector2(transform.position.x - distanceToMidPoint, transform.position.y);

        var newTime = distanceToMidPoint / velocity;

        moving = true;

        mage.DOMove(targetLocation, newTime).SetEase(Ease.Linear).OnComplete(() => {
            moving = false;
            moveRight = true;
        });

        yield return new WaitWhile(() => moving);

        //taunt stuff

        yield return new WaitForSeconds(5f);

        foreach(var projectile in currentProjectiles)
        {
            Destroy(projectile.gameObject);

            yield return new WaitForSeconds(0.5f);
        }

    }

    public IEnumerator StartShooting()
    {
        Debug.Log("Starting SHooting");
        while (moving)
        {
            yield return new WaitForSeconds(shootCooldown);

            Shoot();
        }
    }

    private void Shoot()
    {
        currentProjectiles.Add(Instantiate(projectile, mage.transform.position, Quaternion.identity));
    }
}

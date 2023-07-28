using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FlailAttack : MonoBehaviour, IAttack
{
    [SerializeField] private GameObject oneWays;

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

    public bool shooting { get; private set; }

    private bool shouldShoot = false;

    private bool active = false;

    private bool stopped = false;

    Coroutine lastCoroutine;
    private void OnEnable()
    {
        GetComponent<MageController>().flailAttack += Attack;
        currentProjectiles = new List<GameObject>();
    }

    private void OnDisable()
    {
        GetComponent<MageController>().flailAttack -= Attack;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldShoot && !shooting)
        {
            shooting = true;
            lastCoroutine = StartCoroutine(StartMoving());
        }
    }

    public void Attack(IAction action)
    {
        if(stopped) return;

        oneWays.SetActive(true);

        MageFlailAction act = (MageFlailAction)action;

        this.mage = act.mage;
        this.projectile = act.projectile;
        this.numberOfBackForths = act.numberOfBackForths;
        this.movePoints = act.movePoints;
        this.moveRight = act.moveRight;
        this.moveTime = act.moveTime;
        this.shootCooldown = act.shootCooldown;

        shooting = true;
        shouldShoot = true;

        active = true;
        lastCoroutine = StartCoroutine(StartMoving());
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

        StartCoroutine(DestroyProjectiles());

        yield return new WaitWhile(() => currentProjectiles.Count > 0);

        shooting = false;
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

    public void StopAttack()
    {
        if (!active) return;

        stopped = true;

        DOTween.Kill(mage.transform);

        StopCoroutine(lastCoroutine);

        //Calculates total distance from starting to end position
        var fullDistance = Vector2.Distance(movePoints[0].position, movePoints[1].position);

        //Calculate velocity of object given the full distance and move time
        var velocity = fullDistance / moveTime;

        shouldShoot = false;
        shooting = false;

        var distanceToMidPoint = fullDistance / 2;

        Vector2 targetLocation = transform.position.x < 0 ? new Vector2(transform.position.x + distanceToMidPoint, transform.position.y) :
            new Vector2(transform.position.x - distanceToMidPoint, transform.position.y);

        var newTime = distanceToMidPoint / velocity;

        moving = true;

        mage.DOMove(targetLocation, newTime).SetEase(Ease.Linear).OnComplete(() => {
            moving = false;
            moveRight = true;
        });

        StartCoroutine(DestroyProjectiles());
    }

    /// <summary>
    /// Destroys all current bouncy projectiles with a slight delay with each one
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyProjectiles()
    {
        for(int i = 0; i < currentProjectiles.Count; i++)
        {
            Destroy(currentProjectiles[i]);

            yield return new WaitForSeconds(0.5f);
        }

        currentProjectiles.Clear();
        active = false;
    }

    public bool GetIsActive()
    {
        return active;
    }
}

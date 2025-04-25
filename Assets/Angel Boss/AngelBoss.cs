using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngelBoss : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private AudioSource bossMusic;
    [SerializeField] private Slider healthBar;
    [SerializeField] ParticleSystem bossHitParticles;
    [SerializeField] AudioSource bossHitAudioSource;
    [SerializeField] private Float floatController;
    [SerializeField] private GroundChanges groundChanger;


    [Header("Attacks")]
    private const string DASH_ATTACK = "dash";
    private const string WAVE_ATTACK = "wave";

    List<string> attacks;

    private int lastAttack = 0;
    private string currentAttack;

    [Header("Ground Slam")]
    [SerializeField] private bool WaveAttack = false;
    private GroundSlam groundSlam;

    [Header("Dash Attack")]
    [SerializeField] private bool dashing = false;
    [SerializeField] private bool midDash = false;
    [SerializeField] private float DashTime = 1f;
    [SerializeField] private int numberOfDashes = 7;
    private int dashCounter = 0;

    [SerializeField] private GameObject topLeftPoint;
    [SerializeField] private GameObject midLeftPoint;
    [SerializeField] private GameObject botLeftPoint;
    [SerializeField] private GameObject topRightPoint;
    [SerializeField] private GameObject midRightPoint;
    [SerializeField] private GameObject botRightPoint;
    [SerializeField] private GameObject topDashPoint;
    [SerializeField] private GameObject bottomDashPoint;

    [SerializeField] private float elapsedTime = 0;
    [SerializeField] private int numDashPoints = 8;
    [SerializeField] private int lastPath = -1;

    [SerializeField] private GameObject startingPoint;
    [SerializeField] private GameObject endingPoint;

    [SerializeField] private GameObject dashIndicator;
    private GameObject indicator;
    private Transform indicatorEndPoint;

    [Header("Stats")]
    [SerializeField] private float currentHealth = 0f;
    [SerializeField] private int phase = 1;
    [SerializeField] private bool defeated = false;
    [SerializeField] private bool attacking = false;


    [Header("Fly Away")]
    [SerializeField] private GameObject flyAwayPoint;
    [SerializeField] private Transform originalStartPoint;
    [SerializeField] private float flyTime = 2f;
    [SerializeField] private float elapsedFlightTime = 0f;
    [SerializeField] private bool flyAway = false;
    [SerializeField] private bool midFlight = false;
    [SerializeField] private Vector3 currentPosition;

    [Header("Ground")]
    [SerializeField] private bool destroyGround = false;
    [SerializeField] private bool destroyHelperPlatforms = false;
    [SerializeField] private bool destroyPlatforms = false;
    [SerializeField] private GameObject floorBottomLayer;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Top Bound")]
    [SerializeField] private GameObject topBound;

    private Aimer aimer;

    private bool aiming = false;

    // Start is called before the first frame update
    void Start()
    {
        attacks = new List<string>();

        //Add attack identifiers to attacks list
        attacks.Add(WAVE_ATTACK);
        attacks.Add(DASH_ATTACK);

        aimer = GetComponent<Aimer>();
        groundSlam = GetComponent<GroundSlam>();

        //Testing attacks
        dashing = true;

        StartCoroutine(StartFlying());
    }

    // Update is called once per frame
    void Update()
    {
        if (groundChanger.DestroyGround) return;

        //Makes sure the boss flys away before starting any attacks
        if (flyAway)
        {
            if (!midFlight)
            {
                midFlight = true;
                StartCoroutine(StartFlying());
            }
            return;
        }

        if (dashing)
        {
            if (!midDash)
            {
                floatController.ShouldFloat = false;
                midDash = true;

                //Chooses a random point for the boss to charge from
                //Avoids picking the same path or reverse path (top right to bottom left and bottom left to top right) twice in a row
                int randomStartingPoint = Random.Range(0, numDashPoints);

                if(lastPath % 2 == 0)
                {
                    while(randomStartingPoint == lastPath || randomStartingPoint == lastPath + 1)
                    {
                        randomStartingPoint = Random.Range(0, numDashPoints);
                    }
                }
                else
                {
                    while (randomStartingPoint == lastPath || randomStartingPoint == lastPath - 1)
                    {
                        randomStartingPoint = Random.Range(0, numDashPoints);
                    }
                }

                switch (randomStartingPoint)
                {
                    case 0:
                        startingPoint = topLeftPoint;
                        endingPoint = botRightPoint;
                        break;

                    case 1:
                        startingPoint = botRightPoint;
                        endingPoint = topLeftPoint;
                        break;

                    case 2:
                        startingPoint = botLeftPoint;
                        endingPoint = topRightPoint;
                        break;

                    case 3:
                        startingPoint = topRightPoint;
                        endingPoint = botLeftPoint;
                        break;

                    case 4:
                        startingPoint = bottomDashPoint;
                        endingPoint = topDashPoint;
                        break;

                    case 5:
                        startingPoint = topDashPoint;
                        endingPoint = bottomDashPoint;
                        break;

                    case 6:
                        startingPoint = midRightPoint;
                        endingPoint = midLeftPoint;
                        break;

                    case 7:
                        startingPoint = midLeftPoint;
                        endingPoint = midRightPoint;
                        break;
                }


                lastPath = randomStartingPoint;
                
                //Calculate direction to player
                Vector2 playerDirection = player.transform.position - transform.position;

                //Use direction to calculate look rotation
                Quaternion rotation = Quaternion.LookRotation(playerDirection);

                transform.position = startingPoint.transform.position;

                //Instantiate indicator with inital rotation
                indicator = Instantiate(dashIndicator, startingPoint.transform.position, Quaternion.Euler(0, 0, rotation.z));
                indicatorEndPoint = indicator.transform.GetChild(0);

                aimer.TakeAim(indicator);

                StartCoroutine(StartDashing());


            }
        }

        if (WaveAttack)
        {
            //Enable ground slam attack spawning
            if (!groundSlam.isActiveAndEnabled)
            {
                groundSlam.enabled = true;
            }


        }
    }

    public IEnumerator StartFlying()
    {
        floatController.ShouldFloat = false;

        transform.DOMove(flyAwayPoint.transform.position, flyTime);

        yield return new WaitUntil(() => (transform.position.x == flyAwayPoint.transform.position.x) && (transform.position.y == flyAwayPoint.transform.position.y));

        //currentAttack = ChooseNextAttack();

        //Testing
        currentAttack = WAVE_ATTACK;
        //currentAttack = DASH_ATTACK;

        if (currentAttack.Equals(DASH_ATTACK))
        {
            groundChanger.DestroyTheGround();
            dashing = true;

            flyAway = false;
            floatController.ShouldFloat = true;
            midFlight = false;
        }
        else if (currentAttack.Equals(WAVE_ATTACK))
        {
            WaveAttack = true;

            transform.DOMove(originalStartPoint.position, flyTime).OnComplete(() =>
            {
                flyAway = false;
                floatController.ShouldFloat = true;
                midFlight = false;
            });
        }

    }

    public IEnumerator StartDashing()
    {
        //Wait for aim time 
        yield return new WaitForSeconds(aimer.aimTime * 0.8f);

        indicator.GetComponent<IndicatorFlash>().StartFlash();

        yield return new WaitForSeconds(aimer.aimTime * 0.4f);
        transform.DOMove(indicatorEndPoint.position, DashTime);

        yield return new WaitForSeconds(DashTime);

        midDash = false;
        elapsedTime = 0;
        floatController.ShouldFloat = true;
        Destroy(indicator);
    }

    public string ChooseNextAttack()
    {
        int randAttack = Random.Range(0, attacks.Count);

        while(randAttack == lastAttack)
        {
            randAttack = Random.Range(0, attacks.Count);
        }

        lastAttack = randAttack;

        return attacks[randAttack];
    }
}

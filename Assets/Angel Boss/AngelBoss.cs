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

    [Header("Dash Attack")]
    [SerializeField] private bool midDash = false;
    [SerializeField] private bool dashing = false;
    [SerializeField] private float DashTime = 1f;
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

    [Header("Stats")]
    [SerializeField] private float currentHealth = 0f;
    [SerializeField] private int phase = 1;
    [SerializeField] private bool defeated = false;
    [SerializeField] private bool attacking = false;


    [Header("Fly Away")]
    [SerializeField] private GameObject flyAwayPoint;
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


    // Start is called before the first frame update
    void Start()
    {
        //Testing attacks
        dashing = true;
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

            elapsedFlightTime += Time.deltaTime;
            float percentComplete = elapsedFlightTime / flyTime;
            transform.position = Vector3.Lerp(currentPosition, flyAwayPoint.transform.position, percentComplete);
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

                transform.rotation = startingPoint.transform.rotation;
                lastPath = randomStartingPoint;

                indicator = Instantiate(dashIndicator, startingPoint.transform.position, Quaternion.Euler(0, 0, startingPoint.transform.eulerAngles.z));

                StartCoroutine(StartDashing(indicator));

                StartCoroutine(DashIndicator());

            }

            //Waits until the indicator is fully there before starting the dash
            if (indicator.GetComponent<SpriteRenderer>().color.a < 0.35)
            {
                var currentOpac = indicator.GetComponent<SpriteRenderer>().color.a;
                currentOpac += Time.deltaTime/2;
                indicator.GetComponent<SpriteRenderer>().color = new Color(indicator.GetComponent<SpriteRenderer>().color.r, indicator.GetComponent<SpriteRenderer>().color.g, indicator.GetComponent<SpriteRenderer>().color.b, currentOpac);
            }
            else
            {
                elapsedTime += Time.deltaTime;
                float percentComplete = elapsedTime / DashTime;
                transform.position = Vector3.Lerp(startingPoint.transform.position, endingPoint.transform.position, percentComplete);
            }
        }

    }

    public IEnumerator DashIndicator()
    {
        yield return null;
    }

    public IEnumerator StartFlying()
    {
        floatController.ShouldFloat = false;

        yield return new WaitUntil(() => (transform.position.x == flyAwayPoint.transform.position.x) && (transform.position.y == flyAwayPoint.transform.position.y));

        groundChanger.DestroyTheGround();

        flyAway = false;
        floatController.ShouldFloat = true;
        midFlight = false;
    }

    public IEnumerator StartDashing(GameObject indicator)
    {
        yield return new WaitUntil(() => (transform.position.x == endingPoint.transform.position.x) && (transform.position.y == endingPoint.transform.position.y));

        midDash = false;
        elapsedTime = 0;
        floatController.ShouldFloat = true;
        Destroy(indicator);
    }
}

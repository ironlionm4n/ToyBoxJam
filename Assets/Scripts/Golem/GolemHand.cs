using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class GolemHand : MonoBehaviour
{
    [Header("Hand Identifier")]
    [SerializeField]
    private bool rightHand = false;

    public bool RightHand { get { return rightHand; } }

    private GolemHandFollow golemHandFollowing;

    private RotateToPlayer playerRotater;

    private ClapAttack clapAttack;
    private WaveSlamAttack waveSlamAttack;

    private CinemachineImpulseSource impulseSource;

    public LayerMask groundMask;

    private bool collision = false;
    private bool waitingForCollision = false;

    private bool waitingForSlam = false;

    private float slamSpeed = 0;

    private void Awake()
    {
        golemHandFollowing = GetComponent<GolemHandFollow>();
        clapAttack = transform.parent.GetComponent<ClapAttack>();
        waveSlamAttack = transform.parent.GetComponent<WaveSlamAttack>();
        playerRotater = GetComponent<RotateToPlayer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(waitingForCollision)
        {
            if (rightHand)
            {
                transform.position -= transform.right * slamSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += transform.right * slamSpeed * Time.deltaTime;
            }
        }

       if(waitingForSlam)
        {
            transform.position -= transform.up * slamSpeed * Time.deltaTime;
        }
    }


    public void SingleClap(float moveBackAmount, float pauseTime, float slamSpeed, float stunTime)
    {
        golemHandFollowing.StopFollowing();
        collision = false;
        this.slamSpeed = slamSpeed;

        StartCoroutine(Clap(moveBackAmount, pauseTime, stunTime));
    }

    public void DoubleClap(float moveBackAmount, float pauseTime, float slamSpeed, Transform player)
    {
        golemHandFollowing.StartConstantFollow();
        playerRotater.StopRotating();

        Vector2 rotation = new Vector2(transform.rotation.x, 0);
        transform.DORotate(rotation, 0.3f);

        StartCoroutine(DoubleSlam(moveBackAmount, pauseTime, slamSpeed, player));
    }


    private IEnumerator DoubleSlam(float moveBackAmount, float pauseTime, float slamSpeed, Transform player)
    {
        yield return new WaitForSeconds(pauseTime * 2);


        golemHandFollowing.StopFollowing();
        transform.DOMove(player.position, slamSpeed * 0.75f);

        yield return new WaitForSeconds(slamSpeed * 0.75f);
        clapAttack.DoubleClapShake();

        yield return new WaitForSeconds(10);

        clapAttack.AttackFinished();
        golemHandFollowing.StopConstantFollow();
        golemHandFollowing.StartFollowing();
        playerRotater.StartRotating();
    }

    private IEnumerator Clap(float moveBackAmount, float pauseTime, float stunTime)
    {
        Vector2 targetLocation = new Vector2();

        float moveBackXVal;
        float offsetMultiplier = 1.75f;

        if (rightHand)
        {
            targetLocation = new Vector2(transform.position.x - (golemHandFollowing.HorizontalOffset* offsetMultiplier), transform.position.y);
            moveBackXVal = transform.position.x + moveBackAmount;
        }
        else
        {
            targetLocation = new Vector2(transform.position.x + (golemHandFollowing.HorizontalOffset * offsetMultiplier), transform.position.y);
            moveBackXVal = transform.position.x - moveBackAmount;
        }

        yield return new WaitForSeconds(0.2f);

        transform.DOMoveX(moveBackXVal, pauseTime);

        //Make sure the collision isn't already true
        collision = false;

        yield return new WaitForSeconds(pauseTime * 2);
        playerRotater.StopRotating();

        //transform.DOMove(targetLocation, doubleSlamSpeed);
        waitingForCollision = true;

        yield return new WaitWhile(() => !collision);
        waitingForCollision = false;

        CameraShakeManager.instance.CameraShake(impulseSource);

        yield return new WaitForSeconds(stunTime);

        clapAttack.AttackFinished();
        golemHandFollowing.StartFollowing();
        playerRotater.StartRotating();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall") || collision.CompareTag("Ground"))
        {

            this.collision = true;
        }
    }

    public void WaveSlam(float slamSpeed, float slamPauseTime, GameObject WavePrefab)
    {
        this.slamSpeed = slamSpeed;

        // stop following the player

        golemHandFollowing.StopFollowing();
        playerRotater.StopRotating();
        playerRotater.ResetRotation();

        // start Coroutine for Wave Slam Attack

        StartCoroutine(WaveSlamRoutine(slamPauseTime, WavePrefab));
    }

    private IEnumerator WaveSlamRoutine(float slamPauseTime, GameObject WavePrefab)
    {
        // TODO - Change to fist sprite

        // slam hand onto ground
        collision = false;
        waitingForSlam = true;

        // wait for ground contact

        yield return new WaitWhile(() => !collision);

        waitingForSlam = false;
        //transform.DOKill();

        // add camera shake
        CameraShakeManager.instance.CameraShake(impulseSource);

        // on ground hit spawn waves on right and left sides

        GameObject currentWave = Instantiate(WavePrefab, transform.position, Quaternion.identity);
        currentWave.GetComponent<GolemMovingWave>().StartMoving(true);

        currentWave = Instantiate(WavePrefab, transform.position, Quaternion.identity);
        currentWave.GetComponent<GolemMovingWave>().StartMoving(false);

        // wait some time before returning hand

        yield return new WaitForSeconds(slamPauseTime);

        golemHandFollowing.StartFollowing();
        playerRotater.StartRotating();

        waveSlamAttack.AttackDone();
    }
}

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

    private CinemachineImpulseSource impulseSource;

    private bool collision = false;
    private bool waitingForCollision = false;

    private float slamSpeed = 0;

    private void Awake()
    {
        golemHandFollowing = GetComponent<GolemHandFollow>();
        clapAttack = transform.parent.GetComponent<ClapAttack>();
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
            transform.position += transform.right * slamSpeed * Time.deltaTime;
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
        clapAttack.DoubleClap();

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
}

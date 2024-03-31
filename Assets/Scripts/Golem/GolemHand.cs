using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GolemHand : MonoBehaviour
{
    [Header("Hand Identifier")]
    [SerializeField]
    private bool rightHand = false;

    public bool RightHand { get { return rightHand; } }

    private GolemHandFollow ghf;

    private ClapAttack clapAttack;

    private void Awake()
    {
        ghf = GetComponent<GolemHandFollow>();
        clapAttack = transform.parent.GetComponent<ClapAttack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void SingleClap(float moveBackAmount, float pauseTime, float slamSpeed)
    {
        ghf.StopFollowing();

        StartCoroutine(Clap(moveBackAmount, pauseTime, slamSpeed));
    }

    public void DoubleClap()
    {
        ghf.StartConstantFollow();


    }

    private IEnumerator Clap(float moveBackAmount, float pauseTime, float slamSpeed)
    {
        Vector2 targetLocation = new Vector2();

        float moveBackXVal;
        float offsetMultiplier = 1.75f;

        if (rightHand)
        {
            targetLocation = new Vector2(transform.position.x - (ghf.HorizontalOffset* offsetMultiplier), transform.position.y);
            moveBackXVal = transform.position.x + moveBackAmount;
        }
        else
        {
            targetLocation = new Vector2(transform.position.x + (ghf.HorizontalOffset * offsetMultiplier), transform.position.y);
            moveBackXVal = transform.position.x - moveBackAmount;
        }

        yield return new WaitForSeconds(0.2f);

        transform.DOMoveX(moveBackXVal, pauseTime);

        yield return new WaitForSeconds(pauseTime * 2);


        transform.DOMove(targetLocation, slamSpeed);

        yield return new WaitForSeconds(slamSpeed * 1.5f);

        clapAttack.AttackFinished();
        ghf.StartFollowing();
    }
}

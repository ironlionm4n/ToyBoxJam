using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GolemHandFollow : MonoBehaviour
{
    [Header("Offsets")]

    [SerializeField]
    private float horizontalOffset = 10f;

    public float HorizontalOffset { get { return horizontalOffset; }}

    [SerializeField]
    private Transform[] xClamps;

    [SerializeField]
    private float followDelay = 0.8f;

    private bool rightHand = false;

    private GolemHand golemHand;

    private Transform player;

    private Vector2 targetLocation;

    private bool following = true;
    private bool constantFollow = false;
    private bool moving = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        golemHand= GetComponent<GolemHand>();

        rightHand = golemHand.RightHand;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(following)
        {
            if (!moving)
            {
                moving = true;
                targetLocation = new Vector2();

                // during normal attacks we want to limit the x values to avoid wall clipping
                if (!constantFollow) { 

                if (rightHand)
                {
                    targetLocation = new Vector2(Mathf.Clamp(player.position.x + horizontalOffset, xClamps[0].position.x, xClamps[1].position.x), player.position.y);
                }
                else
                {
                    targetLocation = new Vector2(Mathf.Clamp(player.position.x - horizontalOffset, xClamps[0].position.x, xClamps[1].position.x), player.position.y);
                }
                }
                else
                {
                    if (rightHand)
                    {
                        targetLocation = new Vector2(player.position.x + horizontalOffset, player.position.y);
                    }
                    else
                    {
                        targetLocation = new Vector2(player.position.x - horizontalOffset, player.position.y);
                    }
                }

                StartCoroutine(Follow());
            }
        }

        
    }


    private IEnumerator Follow()
    {
        float timePassed = 0f;

        Vector2 start = transform.position;
        Vector2 end = targetLocation;

        while (timePassed < followDelay)
        {
            float linearT = timePassed / followDelay; //0 to 1 time

            transform.position = Vector3.Lerp(start, end, linearT);

            timePassed += Time.deltaTime;

            yield return null;

        }

        moving = false;

    }

    public void StopFollowing()
    {
        following = false;
        StopCoroutine(Follow());
    }

    public void StartFollowing()
    {
        following = true;
        moving= false;
    }

    public void StartConstantFollow()
    {
        //StopFollowing();
        constantFollow = true; 

        // set follow delay to near 0 for no delay in tracking player y
        followDelay = 0.05f;
    }

    public void StopConstantFollow()
    {
        constantFollow= false;
        followDelay = 0.3f;
    }
}

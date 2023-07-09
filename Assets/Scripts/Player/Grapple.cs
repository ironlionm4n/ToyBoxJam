using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Grapple : MonoBehaviour
{

    [Header("Sprite")]
    [SerializeField] private Sprite grappleSprite;
    [SerializeField] private GameObject grapplePrefab;

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] private Camera cam;

    [SerializeField] LayerMask layerMask;

    private Vector3 targetPosition;

    private DistanceJoint2D springJoint;

    [Header("Aim Reticle")]
    [SerializeField] private Transform aimReticle;


    [SerializeField] private bool shooting = false;
    [SerializeField] private bool latched = false;
    [SerializeField] private bool returning = false;
    private bool grappling = false;

    [Header("Grapple Variables")]
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float travelSpeed;
    [SerializeField] private float retractSpeed;
    [SerializeField] private float maxRopeLength;
    [SerializeField] private float minRopeLength;
    [SerializeField] private float pullStrength;

    [SerializeField]private bool finishedShooting = false;
    [SerializeField] private bool shortenRope = false;
    [SerializeField] private float pullSpeed = 0.5f;

    [Header("Rope Variables")]
    [SerializeField] private int numberOfPoints = 40;
    [SerializeField] private bool snap = false;
    [SerializeField] private float ropeChangeAmount = 0.05f;

    private Transform snapPoint;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)][SerializeField] private float StartWaveSize = 2;
    float waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField][Range(1, 50)] private float ropeProgressionSpeed = 1;

    float moveTime = 0;

    private Transform currentGrapple;

    private MovingGrappleHook currentMovingHook;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<DistanceJoint2D>();

        lineRenderer.positionCount = numberOfPoints;

        lineRenderer.enabled = false;
        springJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            targetPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            

            if(!returning && !latched && !shooting)
            {
                grappling = true;
                shooting = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopGrappling();
        }

        if (!grappling)
        {
            lineRenderer.enabled = false;
            springJoint.enabled = false;

            return;
        }

        lineRenderer.enabled = true;

        if(returning)
        {
            springJoint.enabled = false;

            return;

        }

        //Gets the direction from the target to the player and normalizes it
        var direction = targetPosition - transform.position;
        direction.Normalize();

        //Gets the length between the player and target position
        var length = Vector2.Distance(transform.position, targetPosition);

        //Makes sure the player is not clicking past the maxTravelDistance mark
        if (length > maxTravelDistance)
        {
            length = maxTravelDistance;
        }

        //Checks for a Raycast hit on the specified layers
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, length, layerMask);


        moveTime += Time.deltaTime;

        if (!snap)
        {
            DrawRope(hit);
        }
        else
        {
            SnapRope(hit);

            if (Input.GetKey(KeyCode.W))
            {
                if (springJoint.enabled && springJoint.distance > minRopeLength)
                {
                    springJoint.distance -= ropeChangeAmount;
                }
            }

            if(Input.GetKey(KeyCode.S))
            {
                if (springJoint.enabled && springJoint.distance < maxRopeLength)
                {
                    springJoint.distance += ropeChangeAmount;
                }
            }
        }

        if (shooting)
        {
            shooting = false;

            StartCoroutine(CheckIfHit(hit));
        }

    }

    public void DrawRope(RaycastHit2D hit)
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            float delta = (float)i / ((float)numberOfPoints - 1f);
            var distance = targetPosition - transform.position;


            Vector2 offset = Vector2.Perpendicular(distance).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 tPosition = Vector2.Lerp(transform.position, targetPosition, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(transform.position, tPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            lineRenderer.SetPosition(i, currentPosition);

            //Once the line is finished moving, set finishedShooting to true || if the distance is greater or equal to max travel distance
            if(currentPosition == (Vector2)targetPosition || Vector2.Distance(transform.position, currentPosition) >= maxTravelDistance)
            {
                if (hit == true)
                {

                    if (hit.collider.gameObject.layer == 9)
                    {
                       SnapRope(hit);
                        snap = true;
                    }
                }

                finishedShooting = true;

                
            }
        }

    }

    public void SnapRope(RaycastHit2D hit)
    {
        if (hit == true)
        {
            lineRenderer.positionCount = 2;

            snapPoint = hit.collider.transform;

            //If the player is hooked to a moving hook, move hook start moving hook
            if (hit.transform.tag.Equals("MovingHook"))
            {
                currentMovingHook = hit.transform.GetComponent<MovingGrappleHook>();
                currentMovingHook.PlayerSnapped();
            }
        }

        if (snapPoint != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, snapPoint.position);
        }

    }

    private void StopGrappling()
    {
        //If the player was hooked to a moving hook, move hook back to starting location
        if(currentMovingHook != null)
        {
            currentMovingHook.PlayerUnSnapped();
            currentMovingHook=null;
        }

        lineRenderer.enabled = false;
        springJoint.enabled = false;
        finishedShooting = false;

        grappling = false;
        shooting = false;
        moveTime = 0;
        lineRenderer.positionCount = numberOfPoints;
        snap = false;
    }

    private IEnumerator CheckIfHit(RaycastHit2D hit)
    {
        //Waits until the line is finished being drawn to the target position
        yield return new WaitWhile(() => !finishedShooting);

        //If something was hit
        if (hit != false)
        {
            Debug.Log(hit.collider);

            //Get the distance to what the rope is snapped to
            var distance = Vector2.Distance(transform.position, snapPoint.position);

            //If the distance is less than the maxRopeLength use the shorter distance 
            if (distance < maxRopeLength)
            {
                springJoint.distance = distance;
            }
            else
            {
                springJoint.distance = maxRopeLength;
            }

            //Enable spring joint and set connectedBody
            springJoint.enabled = true;
            springJoint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
        }
        else
        {
            //Otherwise stop grappling
            StopGrappling();
        }
    }
}

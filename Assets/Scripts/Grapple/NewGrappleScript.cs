using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(DistanceJoint2D))]
public class NewGrappleScript : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private DistanceJoint2D distanceJoint;
    private Camera cam;

    private Vector2 currentTarget;

    [Header("Rope Variables")]
    [SerializeField] 
    private int numberOfPoints = 40;

    [SerializeField]
    private bool snap = false;

    [SerializeField]
    private bool grappling = false;

    [SerializeField] 
    private float ropeChangeAmount = 0.05f;

    [SerializeField]
    private float grappleExtendSpeed = 5f;

    [SerializeField]
    private float maxTravelDistance = 6f;

    Transform snapPoint;
    private MovingGrappleHook currentMovingHook;

    private void Awake()
    {
        //Get the main camera
        cam = Camera.main;   
        
        //Get required components
        lineRenderer = GetComponent<LineRenderer>();
        distanceJoint = GetComponent<DistanceJoint2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            currentTarget = cam.ScreenToWorldPoint(Input.mousePosition);
            grappling = true;
        }

        if (!grappling) return;

        lineRenderer.enabled = true;

        Debug.Log(currentTarget);

        if(!snap)
        {
            snap = true;
            RaycastHit2D hit = new RaycastHit2D();
            StartCoroutine(DrawRope(hit));
        }
        else
        {

        }
    }

    private void StopGrappling()
    {
        //If the player was hooked to a moving hook, move hook back to starting location
        if (currentMovingHook != null)
        {
            currentMovingHook.PlayerUnSnapped();
            currentMovingHook = null;
        }

        ResetLineRendererPositions();

        lineRenderer.enabled = false;
        distanceJoint.enabled = false;

        grappling = false;
        lineRenderer.positionCount = numberOfPoints;
        snap = false;
    }

    void ResetLineRendererPositions()
    {
        lineRenderer.positionCount = numberOfPoints;

        for(int i = 0; i < numberOfPoints; i++)
        {
            lineRenderer.SetPosition(i,  Vector2.zero);
        }
    }

    public IEnumerator DrawRope(RaycastHit2D hit)
    {
        float xChange = (Mathf.Abs(currentTarget.x) - Mathf.Abs(transform.position.x)) / numberOfPoints;
        float yChange = (Mathf.Abs(currentTarget.y) - Mathf.Abs(transform.position.y)) /numberOfPoints;

        for(int i = 0; i < numberOfPoints; i++)
        {
            Vector2 tPosition = new Vector2(transform.position.x + (xChange * i), transform.position.y + (yChange * i));
            lineRenderer.SetPosition(i, tPosition);
            yield return new WaitForSeconds(grappleExtendSpeed);

            if(Vector2.Distance(transform.position, currentTarget) >= maxTravelDistance)
            {
                break;
            }
        }

        //Once the line is finished moving
        if (hit == true)
        {

            if (hit.collider.gameObject.layer == 9)
            {
                SnapRope(hit);
                snap = true;
            }
            else
            {
                StopGrappling();
            }
        }
        else
        {
            Debug.Log("Grapple broke");
            StopGrappling();
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
                currentMovingHook?.PlayerSnapped();
            }
        }

        if (snapPoint != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, snapPoint.position);
        }

    }
}

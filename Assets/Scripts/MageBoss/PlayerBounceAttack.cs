using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounceAttack : MonoBehaviour
{
    [SerializeField] private GameObject oneWays;

    [SerializeField] private GameObject rollerSpawnPoint;
    [SerializeField] private GameObject roller;

    [SerializeField] private int numberOfRollers;

    [SerializeField] private float timeBetweenRollers;


    private List<GameObject> currentRollers;

    public GameObject player { get; private set; }
    public Rigidbody2D playerRB { get; private set; }

    public Jump playerJump { get; private set; }



    private void OnEnable()
    {
        GetComponent<MageController>().bounceEffect += StartBouncingAttack;
        currentRollers = new List<GameObject>();
    }

    private void OnDisable()
    {
        GetComponent<MageController>().bounceEffect -= StartBouncingAttack;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartBouncingAttack(MageBounceAction action)
    {
        player = action.player;

        playerRB = player.GetComponent<Rigidbody2D>();

        playerJump = player.GetComponent<Jump>();

        playerJump.DisableJumping();

        playerJump.BouncyFloorEffect();

        oneWays.SetActive(false);

        playerRB.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);

        StartCoroutine(BouncyAttack());
    }

    private IEnumerator BouncyAttack()
    {
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < numberOfRollers; i++)
        {
            currentRollers.Add(Instantiate(roller, rollerSpawnPoint.transform.position, Quaternion.identity));

            yield return new WaitForSeconds(timeBetweenRollers);
        }

        yield return new WaitForSeconds(10f);

        StartCoroutine(EndAttack());
    }

    private IEnumerator EndAttack()
    {
        for (int i = 0; i < currentRollers.Count; i++)
        {
            Destroy(currentRollers[i]);

            yield return new WaitForSeconds(timeBetweenRollers);
        }

        currentRollers.Clear();
    }
}

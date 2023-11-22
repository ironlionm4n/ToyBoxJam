using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerBounceAttack : MonoBehaviour, IAttack
{
    [SerializeField] private GameObject oneWays;

    [SerializeField] private GameObject rollerSpawnPoint;

    [SerializeField] private GameObject rollerStuff;

    [SerializeField] private int numberOfRollers;

    [SerializeField] private float timeBetweenRollers;

    private GameObject roller;

    private List<GameObject> currentRollers;

    public GameObject player { get; private set; }
    public Rigidbody2D playerRB { get; private set; }

    public Jump playerJump { get; private set; }

    public MageCoinSpawning coinSpawning;

    private bool active;
    private bool stopped = false;
    private void OnEnable()
    {
        GetComponent<MageController>().bounceEffect += Attack;
        currentRollers = new List<GameObject>();
    }

    private void OnDisable()
    {
        GetComponent<MageController>().bounceEffect -= Attack;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack(IAction action)
    {
        if(stopped) return;

        MageBounceAction act = (MageBounceAction)action;

        player = act.player;

        playerRB = player.GetComponent<Rigidbody2D>();

        playerJump = player.GetComponent<Jump>();

        playerJump.DisableJumping();

        playerJump.BouncyFloorEffect();

        oneWays.SetActive(false);

        roller = act.roller;

        if (!playerJump.IsJumping)
        {
            playerRB.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        }
        rollerStuff.SetActive(true);

        active = true;

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

        coinSpawning.SpawnCoin();
    }

    public void StopAttack()
    {
        stopped = true;
        StartCoroutine(EndAttack());
    }

    public IEnumerator EndAttack()
    {
        StopCoroutine(BouncyAttack());

        for (int i = 0; i < currentRollers.Count; i++)
        {
            Destroy(currentRollers[i]);

            yield return new WaitForSeconds(timeBetweenRollers);
        }

        currentRollers.Clear();

        playerJump?.EnableJumping();

        playerJump?.BouncyFloorEffect();

        rollerStuff.SetActive(false);

        active = false;
    }

    public bool GetIsActive()
    {
        return active;
    }
}

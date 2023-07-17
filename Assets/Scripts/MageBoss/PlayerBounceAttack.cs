using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounceAttack : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D bounceMaterial;
    [SerializeField] private PhysicsMaterial2D playerMaterial;

    [SerializeField] private GameObject oneWays;
    [SerializeField] private GameObject ground;

    public GameObject player { get; private set; }
    public Rigidbody2D playerRB { get; private set; }

    public Rigidbody2D groundRB { get; private set; }

    public Jump playerJump { get; private set; }

    public bool bouncy { get; private set; } = false;


    private void OnEnable()
    {
        GetComponent<MageController>().bounceEffect += StartBouncingAttack;
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

        groundRB = ground.GetComponent<Rigidbody2D>();

        playerRB = player.GetComponent<Rigidbody2D>();

        playerJump = player.GetComponent<Jump>();

        //groundRB.sharedMaterial = bounceMaterial;

        playerJump.DisableJumping();

        playerJump.BouncyFloorEffect();

        oneWays.SetActive(false);

        playerRB.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);

        //StartCoroutine(BouncyAttack());
    }

    private IEnumerator BouncyAttack()
    {
        yield return new WaitForSeconds(4f);

        //playerJump.EnableJumping();

        oneWays.SetActive(true);

        //playerRB.sharedMaterial = playerMaterial;
    }
}

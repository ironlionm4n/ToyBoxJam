using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAttackFalling : MonoBehaviour
{
    [SerializeField] private GameObject Wave;
    [SerializeField]private Animation groundStickFlash;
    [SerializeField] private Animator animator;
    [SerializeField] private Animation anim;
    private Rigidbody2D rb;

    private float[] height;
    public int currentHeight;
    private float speed;
    public int numberOfWaves;

    private bool valuesSet = false;

    private GroundSlam parent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //For testing
        //height = new float[] { 1, 2, 4 };
       // numberOfWaves = height.Length;
        //speed = 5f;

        animator = GetComponent<Animator>();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!valuesSet)
        {
            return;
        }

        if(currentHeight >= numberOfWaves)
        {
            parent.WavesComplete();
            Destroy(transform.parent.gameObject);
        }
    }

    /// <summary>
    /// Sets the height, speed, and how many bursts of waves values for the next set of waves
    /// </summary>
    /// <param name="height"></param>
    /// <param name="speed"></param>
    public void SetValues(float[] height, float speed, GroundSlam gs)
    {
        this.height = height;
        this.speed = speed;
        numberOfWaves = height.Length-1; //Ignore last value which is for probability
        currentHeight = 0;
        parent = gs;

        valuesSet = true;
    }

    /// <summary>
    /// Spawns a set of two waves, one moving left + one moving right.
    /// Also sets the height, direction, and speed value of each wave
    /// </summary>
    public void SpawnWaves()
    {
        Wave currentWave = Instantiate(Wave).GetComponent<Wave>();

        currentWave.Spawn(height[currentHeight], transform.position, true, speed, this);

        currentWave = Instantiate(Wave).GetComponent<Wave>();

        currentWave.Spawn(height[currentHeight], transform.position, false, speed, this);

        currentHeight++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            //SpawnWaves();
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetBool("OnGround", true);
        }
    }
}

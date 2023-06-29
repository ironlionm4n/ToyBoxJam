using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : MonoBehaviour
{
    [Header("Ground Slam")]
    [SerializeField] private GameObject dropProjectile;
    public bool wavesDropped = false;
    private float timeBetweenWaves = 3f;
    private float timer = 0;

    List<float[]> phase1HeightPatterns;
    List<float[]> phase2HeightPatterns;
    List<float[]> phase3HeightPatterns;

    private float lastWaveHeight;

    private float maxWaveHeight;
    private float minWaveHeight;

    private float waveSpeed = 5f;

    //Phase 1 Height Patterns which include the height of each pair of waves + the final value is a probablity for how often to be used
    private float[] p1HP1 = new float[] { 1, 2, 2, 3, 2, 2, 1, 0.33f };
    private float[] p1HP2 = new float[] { 2, 1, 3, 2, 3, 1, 2, 0.33f };
    private float[] p1HP3 = new float[] { 2, 2, 2, 3, 0.33f };

    private int nextAttackPattern = 0;

    // Start is called before the first frame update
    void Start()
    {
        phase1HeightPatterns = new List<float[]>();
        phase1HeightPatterns.Add(p1HP1);
        phase1HeightPatterns.Add(p1HP2);
        phase1HeightPatterns.Add(p1HP3);

        phase2HeightPatterns = new List<float[]>();

        phase3HeightPatterns = new List<float[]>();
    }

    // Update is called once per frame
    void Update()
    {
        //Makes sure a wave spawner is not already dropped and active
        if (!wavesDropped)
        {
            //Add a time for a little bit of break between
            if(timer >= timeBetweenWaves && nextAttackPattern < phase1HeightPatterns.Count)
            {
                wavesDropped = true;
                DropWaves();
                nextAttackPattern++;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Drops a WaveAttackFalling spawn object and sets all the values for that object
    /// </summary>
    public void DropWaves()
    {
        GameObject drop = Instantiate(dropProjectile, transform.position, Quaternion.identity);
        WaveAttackFalling wf = drop.transform.GetChild(0).GetComponent<WaveAttackFalling>();
        wf.SetValues(phase1HeightPatterns[nextAttackPattern], waveSpeed, this); 
    }

    public void WavesComplete()
    {
        timer = 0f;
        wavesDropped = false;
    }
}

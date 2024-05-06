using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlickerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject lightsHolder;

    [SerializeField]
    private GameObject SliceAttackPrefab;

    private List<Torch> lights = new List<Torch>();

    private bool attacking = false;
    private bool flickering = false;

    private void Awake()
    {
        // get all the lights in the scene
        foreach(Transform t in lightsHolder.transform)
        {
            lights.Add(t.GetComponent<Torch>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFlickerAttack()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        // randomly choose to do odd or even lights first
        int evenOdd = Random.Range(0, 2);

        Debug.Log(evenOdd);
        if (evenOdd == 0)
        {
            // flicker odd lights
            OddLights();

            yield return new WaitForSeconds(1);

            //attacking = true;

            OddSlices();

            yield return new WaitForSeconds(1);

            // flicker even lights

            flickering = true;
            EvenLights();

            yield return new WaitForSeconds(1f);

            //attacking = true;
            EvenSlices();

            yield return new WaitForSeconds(1.5f);
        }
        else
        {
            // flicker even lights
            EvenLights();

            yield return new WaitForSeconds(1);

            EvenSlices();

            yield return new WaitForSeconds(1);

            // flicker odd lights

            OddLights();

            yield return new WaitForSeconds(1);


            OddSlices();

            yield return new WaitForSeconds(1.5f);

        }

        foreach(Torch torch in lights)
        {
            torch.StartCoroutine(torch.Relight());
        }
    }

    private void OddLights()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (i % 2 != 0)
            {
                lights[i].StartFlickering();
            }
        }

        flickering = false;

    }

    private void OddSlices()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (i % 2 != 0)
            {
                Instantiate(SliceAttackPrefab, lights[i].transform);
            }
        }
    }

    private void EvenLights()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (i % 2 == 0)
            {
                lights[i].StartFlickering();
            }
        }

        flickering = false;
    }

    private void EvenSlices()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (i % 2 == 0)
            {
                Instantiate(SliceAttackPrefab, lights[i].transform);
            }
        }
    }
}

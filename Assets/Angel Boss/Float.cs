using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Float : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private GameObject topPoint;
    [SerializeField] private GameObject bottomPoint;
    [SerializeField] private float timeBetweenBobs = 2f;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private bool bobbingUp = false;
    [SerializeField] private float bufferTime = 0.5f;
    [SerializeField] private bool shouldFloat = true;

    public bool ShouldFloat {get { return shouldFloat; } set { shouldFloat = value; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!shouldFloat) return;

        //Makes boss bob while floating
        if (!bobbingUp)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / timeBetweenBobs;
            transform.position = Vector3.Lerp(topPoint.transform.position, bottomPoint.transform.position, percentComplete);
        }
        else
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / timeBetweenBobs;
            transform.position = Vector3.Lerp(bottomPoint.transform.position, topPoint.transform.position, percentComplete);
        }

        if (elapsedTime >= timeBetweenBobs)
        {
            bobbingUp = !bobbingUp;
            elapsedTime = 0;
            StartCoroutine(Buffer());
        }

    }

    public IEnumerator Buffer()
    {
        yield return new WaitForSeconds(bufferTime);
    }
}

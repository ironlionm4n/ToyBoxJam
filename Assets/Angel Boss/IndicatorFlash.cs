using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorFlash : MonoBehaviour
{
    private SpriteRenderer sr;

    private bool startFlashing;
    private bool flashing = false;
    private float flashTime = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startFlashing)
        {
            return;
        }

        if (!flashing)
        {
            flashing = true;
            StartCoroutine(Flash());
        }
    }

    public void StartFlash()
    {
        startFlashing = true;
    }

    private IEnumerator Flash()
    {
        yield return new WaitForSeconds(flashTime);

        sr.enabled = false;

        yield return new WaitForSeconds(flashTime);

        sr.enabled = true;

        flashing = false;
    }
}

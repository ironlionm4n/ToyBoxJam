using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButtonFlash : MonoBehaviour
{
    [SerializeField] private bool flashing = false;

    // Update is called once per frame
    void Update()
    {
        if (!flashing)
        {
            flashing = true;
            StartCoroutine(Flash());
        }
    }

    public IEnumerator Flash()
    {

        gameObject.GetComponent<Image>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<Image>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        flashing = false;
    }
}

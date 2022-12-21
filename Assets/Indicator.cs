using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private SpriteRenderer sp1;
    [SerializeField] private SpriteRenderer sp2;

    [Header("Variables")]
    [SerializeField] private bool flashing = false;
    [SerializeField] private float flashSpeed = 0.5f;
    [SerializeField] private float flashTime = 5f;
    [SerializeField] private float flashTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(!flashing)
        {
            StartCoroutine(Flash());
        }
    }

    public IEnumerator Flash()
    {
        flashing= true;

        flashTimer = 0f;

        while (flashTimer < flashTime)
        {
            sp1.enabled = false;
            sp2.enabled = false;

            yield return new WaitForSeconds(flashSpeed/2);

            sp1.enabled = true;
            sp2.enabled = true;

            flashTimer += flashSpeed;

            yield return new WaitForSeconds(flashSpeed/2);
        }

        flashing = false;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

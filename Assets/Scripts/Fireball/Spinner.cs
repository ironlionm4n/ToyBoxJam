using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject target;

    [Header("Variables")]
    [SerializeField] private float spinSpeed = 75f;
    [SerializeField] private float spinTime = 9f;
    [SerializeField] private float spinTimer = 0f;
    [SerializeField] private bool activated = false;

    [Header("Fireball Rows")]
    [SerializeField] private GameObject[] NorthRow;
    [SerializeField] private GameObject[] EastRow;
    [SerializeField] private GameObject[] SouthRow;
    [SerializeField] private GameObject[] WestRow;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Activate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            Vector2 direction = (Vector2)target.transform.position - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.right).z;

            rb.angularVelocity = -rotateAmount * spinSpeed;
            
            spinTimer += Time.deltaTime;
        }

    }

    public IEnumerator Flash()
    {
        yield return new WaitForSeconds(0.5f);

        while (activated)
        {
            for (int i = NorthRow.Length - 1; i > -1; i--)
            {
                NorthRow[i].gameObject.SetActive(false);
                EastRow[i].gameObject.SetActive(false);
                SouthRow[i].gameObject.SetActive(false);
                WestRow[i].gameObject.SetActive(false);
            }
                
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < NorthRow.Length; i++)
            {
                NorthRow[i].gameObject.SetActive(true);
                EastRow[i].gameObject.SetActive(true);
                SouthRow[i].gameObject.SetActive(true);
                WestRow[i].gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator Activate()
    {
        for(int i =0; i < NorthRow.Length; i++)
        {
            NorthRow[i].gameObject.SetActive(true);
            EastRow[i].gameObject.SetActive(true);
            SouthRow[i].gameObject.SetActive(true);
            WestRow[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.75f);

        activated = true;
        //StartCoroutine(Flash());
    }

    public IEnumerator Deactivate()
    {
        activated= false;
        for (int i = NorthRow.Length-1; i > -1; i--)
        {
            NorthRow[i].gameObject.SetActive(false);
            EastRow[i].gameObject.SetActive(false);
            SouthRow[i].gameObject.SetActive(false);
            WestRow[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DeactivateSpinner()
    {
        if (activated)
        {
            StartCoroutine(Deactivate());
        }
    }
}

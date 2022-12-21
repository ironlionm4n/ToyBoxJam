using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject target;

    [Header("Variables")]
    [SerializeField] private float spinSpeed = 5f;
    [SerializeField] private float spinTime = 10f;
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
            if(spinTimer >= spinTime)
            {
                activated = false;
                StartCoroutine(Deactivate());
            }

            Vector2 direction = (Vector2)target.transform.position - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.right).z;

            rb.angularVelocity = -rotateAmount * spinSpeed;
            
            spinTimer += Time.deltaTime;
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

        activated = true;
    }

    public IEnumerator Deactivate()
    {
        for (int i = NorthRow.Length-1; i > -1; i--)
        {
            NorthRow[i].gameObject.SetActive(false);
            EastRow[i].gameObject.SetActive(false);
            SouthRow[i].gameObject.SetActive(false);
            WestRow[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

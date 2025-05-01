using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneRingAttack : MonoBehaviour
{
    [SerializeField] private float DestroyTime = 10f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyRing());
    }

    private IEnumerator DestroyRing()
    {
        yield return new WaitForSeconds(DestroyTime);

        Destroy(gameObject);
    }
}

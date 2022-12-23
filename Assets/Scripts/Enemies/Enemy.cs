using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float health = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(gameObject.GetComponent<BossStats>() != null)
        {
            gameObject.GetComponent<BossStats>().Hit(damage);
        }

        if(health <= 0)
        {
            if(gameObject.GetComponent<BatController>() != null)
            {
                gameObject.GetComponent<BatController>().Dead();
            }
        }
    }
}

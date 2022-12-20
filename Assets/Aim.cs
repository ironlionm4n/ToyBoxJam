using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Aim : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject pivotPoint;
    [SerializeField] private GameObject coinSpawnPoint;
    [SerializeField] private Camera mainCamera;

    [Header("Variables")]
    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private float rotZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - pivotPoint.transform.position;

        rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if(rotZ < -30 && rotZ > -140)
        {
            if((mousePosition.x) < (transform.position.x))
            {
                rotZ = -150;
            } else if ((mousePosition.x) > (transform.position.x))
            {
                rotZ = -30;
            }
        }

        pivotPoint.transform.rotation = Quaternion.Euler(0, 0, rotZ-90);
    }
}

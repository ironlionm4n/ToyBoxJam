using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private float minX = -16.9f;
    [SerializeField] private float maxX = 17.5f;
    [SerializeField] private float minY = 1.5f;
    [SerializeField] private float maxY = 5f;

    [Header("Cutscene Management")]
    [SerializeField] private bool inCutscene = false;
    [SerializeField] private bool cutsceneEnding = false;
    [SerializeField] private float cameraMoveTime = 3f;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private GameObject cameraStopPosition;
    [SerializeField] private Vector3 cameraStartPosition;

    public bool InCutscene { get { return inCutscene; } set { inCutscene = value; } }

    private void OnEnable()
    {
        cameraStartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (inCutscene)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / cameraMoveTime;

            transform.position = Vector3.Lerp(cameraStartPosition, cameraStopPosition.transform.position, percentComplete);
            return;
        }

        if (cutsceneEnding)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / cameraMoveTime;

            transform.position = Vector3.Lerp(cameraStopPosition.transform.position, new Vector3(minX, minY), percentComplete);

            if(transform.position.x <= minX)
            {
                cutsceneEnding= false;
            }

            return;
        }

        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, minX, maxX),
            Mathf.Clamp(player.transform.position.y, minY, maxY), -10);
    }

    public void CutSceneEnding()
    {
        inCutscene = false;
        elapsedTime= 0;
        cameraMoveTime = 1.5f;
        cutsceneEnding = true;
    }

    public bool GetCutsceneEnding()
    {
        return cutsceneEnding;
    }
}

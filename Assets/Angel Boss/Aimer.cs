using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aimer : MonoBehaviour
{
    public Quaternion aimRotation { get; private set; }

    [SerializeField] private Transform player;

    public float aimTime { get; private set; } = 2.5f;

    public bool aiming { get; private set; }

    private GameObject indicator;

    // Update is called once per frame
    void Update()
    {
        if (aiming && indicator != null)
        {
            // Calculate the direction from the beam's origin to the player
            Vector3 targetDirection = player.transform.position - transform.position;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            // Rotate the beam's sprite to face the player
            indicator.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }

    /// <summary>
    /// Takes in an indicator that will track the play and mark where the boss is going to dash
    /// </summary>
    /// <param name="ind"></param>
    public void TakeAim(GameObject ind)
    {
        if (ind != null)
        {
            indicator = ind;
            StartCoroutine(StartAiming());
        }
    }

    public IEnumerator StartAiming()
    {
        aiming = true;

        indicator.transform.GetComponent<SpriteRenderer>().DOFade(0.3f, aimTime);

        yield return new WaitForSeconds(aimTime);

        aiming = false;
    }
}

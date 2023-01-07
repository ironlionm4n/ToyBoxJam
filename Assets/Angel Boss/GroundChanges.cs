using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChanges : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Ground")]
    [SerializeField] private GameObject floorTopLayer;
    [SerializeField] private GameObject floorLayerTwo;
    [SerializeField] private GameObject floorLayerThree;
    [SerializeField] private GameObject floorBottomLayer;

    [Header("Platforms")]
    [SerializeField] private GameObject helperPlatforms;
    [SerializeField] private GameObject bossPlatforms;

    [Header("States")]
    [SerializeField] private bool destroyGround = false;
    [SerializeField] private bool destroyHelperPlatforms = false;
    [SerializeField] private bool destroyPlatforms = false;
    [SerializeField] private bool buildGround = false;
    [SerializeField] private bool buildHelperPlatforms = false;
    [SerializeField] private bool buildPlatforms = false;

    [Header("Indicators")]
    [SerializeField] private GameObject groundIndicators;
    [SerializeField] private GameObject helperPlatformIndicators;

    public bool DestroyGround { get { return destroyGround; } set { destroyGround = value; } }
    public bool DestroyHelperPlatforms { get { return destroyHelperPlatforms; } set { destroyHelperPlatforms = value; } }
    public bool DestroyPlatforms { get { return destroyPlatforms; } set { destroyPlatforms = value; } }
    public bool BuildGround { get { return buildGround; } set { buildGround = value; } }
    public bool BuildHelperPlatforms { get { return buildHelperPlatforms; } set { buildHelperPlatforms = value; } }
    public bool BuildPlatforms { get { return buildPlatforms; } set { buildPlatforms = value; } }


    [Header("Checkers")]
    [SerializeField] private GameObject destroyChecker;


    public IEnumerator StartBuildGround()
    {
        yield return null;
        buildGround = false;
    }

    public IEnumerator StartDestroyGround()
    {
        StartCoroutine(FlashGroundIndicators());

        //Checks if the player is above the ground before destroying it
        yield return new WaitUntil(() => player.transform.position.y >= destroyChecker.transform.position.y);

        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => player.transform.position.y >= destroyChecker.transform.position.y);

        floorBottomLayer.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        floorLayerThree.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        floorLayerTwo.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        floorTopLayer.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        helperPlatforms.SetActive(false);

        yield return null;
        destroyGround = false;
    }

    public IEnumerator StartBuildHelperPlatforms()
    {
        yield return null;
        buildHelperPlatforms = false;
    }

    public IEnumerator StartDestroyHelperPlatforms()
    {
        yield return null;
        destroyHelperPlatforms = false;
    }

    public IEnumerator StartBuildBossPlatforms()
    {
        yield return null;
        buildPlatforms = false;
    }

    public IEnumerator StartDestroyBossPlatforms()
    {
        yield return null;
        destroyPlatforms = false;
    }

    public IEnumerator FlashGroundIndicators()
    {
        while (destroyGround)
        {
            groundIndicators.SetActive(true);

            yield return new WaitForSeconds(0.3f);

            groundIndicators.SetActive(false);

            yield return new WaitForSeconds(0.3f);

        }

        groundIndicators.SetActive(false);
    }

    public IEnumerator FlashHelperIndicators()
    {
        while (destroyHelperPlatforms)
        {
            helperPlatformIndicators.SetActive(true);

            yield return new WaitForSeconds(0.3f);

            helperPlatformIndicators.SetActive(false);

            yield return new WaitForSeconds(0.3f);

        }

        helperPlatformIndicators.SetActive(false);
    }

    public void DestroyTheGround()
    {
        destroyGround = true;
        StartCoroutine(StartDestroyGround());
    }

    public void BuildTheGround()
    {
        buildGround = true;
        StartCoroutine(StartBuildGround());
    }

    public void DestroyTheHelperPlatforms()
    {
        destroyHelperPlatforms= true;
        StartCoroutine(StartDestroyHelperPlatforms());
    }

    public void BuildTheHelperPlatforms()
    {
        buildHelperPlatforms = true;
        StartCoroutine(StartBuildHelperPlatforms());
    }

    public void DestroyTheBossPlatforms()
    {
        destroyPlatforms = true;
        StartCoroutine(StartDestroyBossPlatforms());
    }

    public void BuildTheBossPlatforms()
    {
        buildPlatforms = true;
        StartCoroutine(StartBuildBossPlatforms());
    }

}

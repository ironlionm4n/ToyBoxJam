using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    private Light2D light2D;

    private float startingIntensity;

    public float intensityChangeAmount = 0.1f;
    public float flickerEndIntensity = 1f;



    private int numberOfFlickers = 2;

    // have to use this to set the falloff value as it's get only
    private static FieldInfo m_FalloffField = typeof(Light2D).GetField("m_FalloffIntensity", BindingFlags.NonPublic | BindingFlags.Instance);


    private void Awake()
    {
        light2D = GetComponentInChildren<Light2D>();
        startingIntensity = light2D.falloffIntensity;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFlickering()
    {
        StartCoroutine(FlickerRoutine());
    }

    public IEnumerator FlickerRoutine()
    {
        // lower and raise the intensity quickly to give off a flicker effect

        for (int i = 0; i < numberOfFlickers; i++)
        {
            while (light2D.falloffIntensity < flickerEndIntensity)
            {
                m_FalloffField.SetValue(light2D, light2D.falloffIntensity + intensityChangeAmount);

                yield return null;
            }



            while (light2D.falloffIntensity > startingIntensity)
            {
                m_FalloffField.SetValue(light2D, light2D.falloffIntensity - intensityChangeAmount);

                yield return null;
            }
        }

        while (light2D.intensity > 0)
        {
            light2D.intensity -= intensityChangeAmount;

            yield return null;
        }
    }

    private void Extinguish()
    {
        while(light2D.intensity > 0)
        {
            light2D.intensity -= intensityChangeAmount;
        }
    }



    public IEnumerator Relight()
    {
        // max falloff for no visibility at first
        m_FalloffField.SetValue(light2D, 1);

        // then reset the intensity
        light2D.intensity = 2;

        // then increment the falloff

        while (light2D.falloffIntensity > startingIntensity)
        {
            m_FalloffField.SetValue(light2D, light2D.falloffIntensity - intensityChangeAmount/4);
            yield return null;
        }
    }
}

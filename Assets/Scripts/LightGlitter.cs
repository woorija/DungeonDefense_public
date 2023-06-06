using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightGlitter : MonoBehaviour
{
    Light2D light2d;
    float intensity;
    bool isIncrease;

    [SerializeField] float intensity_min;
    [SerializeField] float intensity_max;
    [SerializeField] float variation;

    [SerializeField] int count; // -1이면 무한반복
    int currentcount;
    private void Awake()
    {
        light2d = GetComponent<Light2D>();
        intensity = intensity_min;
        isIncrease= true;
    }
    private void OnEnable()
    {
        currentcount = count;
    }
    private void Update()
    {
        if (currentcount != 0)
        {
            Glittering();
        }
    }
    void Glittering()
    {
        if (isIncrease)
        {
            IncreaseIntensity();
        }
        else
        {
            decreaseIntensity();
        }
        light2d.intensity = intensity;
    }
    void IncreaseIntensity()
    {
        intensity += variation;
        if(intensity > intensity_max)
        {
            intensity = intensity_max;
            isIncrease = false;
        }
    }
    void decreaseIntensity()
    {
        intensity -= variation;
        if(intensity < intensity_min )
        {
            intensity = intensity_min;
            currentcount--;
            if (currentcount == 0)
            {
                gameObject.SetActive(false);
            }
            isIncrease = true;
        }
    }
}

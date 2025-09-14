using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MasterControllPlanet : MonoBehaviour{
    
    [Header("Wavy motion settings")]
    public float earthWaveAmplitude = 2f;
    public float earthWaveFrequency = 2f;
    public float moonWaveAmplitude = 1.5f;
    public float moonWaveFrequency = 4f;
    

    private float timeElapsed = 0f;

    [Header("Earth settings")]
    public float earthRotateSpeed = 10f;
    public float earthRevolveSpeed = 10f;
    public float earthDistanceFormSun = 10f;
    public GameObject earth;


    [Header("Moon settings")]
    public float moonRotateSpeed = 10f;
    public float moonRevolveSpeed = 10f;
    public float moonDistanceFormEarth = 10f;
    public GameObject moon;


    [Header("Sun settings")]
    public float sunRotateSpeed = 10f;
    public GameObject sun;


    void FixedUpdate()
    {
        // Add a TrailRenderer to the moon to leave a trace
        if (moon != null && moon.GetComponent<TrailRenderer>() == null)
        {
            var trail = moon.AddComponent<TrailRenderer>();
            trail.time = 1000f; // Large value to keep the trail for a long time
            trail.startWidth = 0.1f;
            trail.endWidth = 0.05f;
            trail.material = new Material(Shader.Find("Sprites/Default"));
            trail.startColor = Color.white;
            trail.endColor = Color.white;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            trail.receiveShadows = false;
        }

        if (earth != null && earth.GetComponent<TrailRenderer>() == null)
        {
            var trail = earth.AddComponent<TrailRenderer>();
            trail.time = 1000f; // Large value to keep the trail for a long time
            trail.startWidth = 0.1f;
            trail.endWidth = 0.05f;
            trail.material = new Material(Shader.Find("Sprites/Default"));
            trail.startColor = Color.white;
            trail.endColor = Color.white;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            trail.receiveShadows = false;
        }

        timeElapsed += Time.deltaTime;

        // Sun rotation
        sun.transform.Rotate(Vector3.up, sunRotateSpeed * Time.deltaTime);

        // Earth rotation and revolution
        earth.transform.Rotate(Vector3.up, earthRotateSpeed * Time.deltaTime);
        float earthRevolveAngle = timeElapsed * earthRevolveSpeed;
        float earthX = Mathf.Cos(earthRevolveAngle * Mathf.Deg2Rad) * earthDistanceFormSun;
        float earthZ = Mathf.Sin(earthRevolveAngle * Mathf.Deg2Rad) * earthDistanceFormSun;
        float earthY = Mathf.Sin(timeElapsed * earthWaveFrequency) * earthWaveAmplitude;
        earth.transform.position = new Vector3(earthX, earthY, earthZ);

        // Moon rotation and revolution
        moon.transform.Rotate(Vector3.up, moonRotateSpeed * Time.deltaTime);
        float moonRevolveAngle = timeElapsed * moonRevolveSpeed;
        float moonX = Mathf.Cos(moonRevolveAngle * Mathf.Deg2Rad) * moonDistanceFormEarth;
        float moonZ = Mathf.Sin(moonRevolveAngle * Mathf.Deg2Rad) * moonDistanceFormEarth;
        float moonY = Mathf.Sin(timeElapsed * moonWaveFrequency) * moonWaveAmplitude;
        moon.transform.position = new Vector3(earth.transform.position.x + moonX, earth.transform.position.y + moonY, earth.transform.position.z + moonZ);
    }
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class HorrorGameManager : MonoBehaviour
{
    [Header("Lighting Setup")]
    public Light[] roomLights;
    public Light ambientLight;
    public Volume postProcessVolume;
    
    [Header("Player References")]
    public FPSController playerController;
    public FlashLightController flashlightController;
    
    [Header("Cube Movement")]
    public GameObject[] cubes;
    public float cubeSpeed = 2f;
    public float detectionRange = 15f;
    public float destroyDelay = 3f;
    
    [Header("Effects")]
    public ParticleSystem destroyParticles;
    public AudioSource flickerSound;
    public AudioSource destroySound;
    
    [Header("Flare System")]
    public GameObject flarePrefab;
    public float flareIntensity = 5f;
    public float flareBrightnessSpeed = 1f;
    public string nextLevelName = "NextLevel";
    
    private Camera playerCamera;
    private List<GameObject> activeCubes;
    private Coroutine lightFlickerCoroutine;

    void Start()
    {
        // Find player components if not assigned
        if (playerController == null)
            playerController = FindObjectOfType<FPSController>();
        
        if (flashlightController == null)
            flashlightController = FindObjectOfType<FlashLightController>();
            
        if (playerController != null)
            playerCamera = playerController.GetPlayerCamera();
            
        SetupInitialLighting();
        SetupCubes();
        StartLightFlicker();
    }
    
    void SetupInitialLighting()
    {
        // Set ambient to black
        RenderSettings.ambientLight = Color.black;
        RenderSettings.ambientMode = AmbientMode.Flat;
        
        // Make atmosphere dark
        if (postProcessVolume != null)
        {
            // You can adjust post-processing here for darker atmosphere
        }
    }
    
    void SetupCubes()
    {
        activeCubes = new List<GameObject>();
        foreach (GameObject cube in cubes)
        {
            if (cube != null)
            {
                activeCubes.Add(cube);
                // Add rigidbody if not present for movement
                if (cube.GetComponent<Rigidbody>() == null)
                {
                    cube.AddComponent<Rigidbody>();
                    cube.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
    }
    
    void StartLightFlicker()
    {
        if (lightFlickerCoroutine != null)
            StopCoroutine(lightFlickerCoroutine);
        lightFlickerCoroutine = StartCoroutine(FlickerLights());
    }
    
    IEnumerator FlickerLights()
    {
        while (true)
        {
            // Random delay between flickers
            yield return new WaitForSeconds(Random.Range(0.1f, 5f));
            
            // Choose random lights to flicker
            foreach (Light light in roomLights)
            {
                if (light != null && Random.value > 0.5f)
                {
                    StartCoroutine(FlickerSingleLight(light));
                }
            }
        }
    }
    
    IEnumerator FlickerSingleLight(Light light)
    {
        float originalIntensity = light.intensity;
        bool originalState = light.enabled;
        
        // Play flicker sound
        if (flickerSound != null)
            flickerSound.Play();
        
        // Flicker pattern
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            light.enabled = !light.enabled;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));
        }
        
        // Sometimes stay off
        if (Random.value > 1f)
        {
            light.enabled = false;
        }
        else
        {
            light.enabled = originalState;
            light.intensity = originalIntensity;
        }
    }
    
    void Update()
    {
        MoveCubesNearPlayer();
        HandleFlashlightTargeting();
        CheckForFlareInput();
    }
    
    void HandleFlashlightTargeting()
    {
        if (flashlightController == null || !flashlightController.IsFlashlightOn()) 
            return;
        
        // Check each active cube to see if flashlight is hitting it
        foreach (GameObject cube in activeCubes.ToArray()) // ToArray to avoid modification during iteration
        {
            if (cube == null) continue;
            
            if (flashlightController.IsHittingObject(cube))
            {
                // Start destruction countdown
                Cube cubeScript = cube.GetComponent<Cube>();
                if (cubeScript != null)
                {
                    cubeScript.StartDestruction();
                }
                
                StartCoroutine(DestroyCubeAfterDelay(cube));
            }
        }
    }
    
    void MoveCubesNearPlayer()
    {
        if (playerCamera == null) return;
        
        Vector3 playerPos = playerCamera.transform.position;
        
        foreach (GameObject cube in activeCubes)
        {
            if (cube == null) continue;
            
            float distance = Vector3.Distance(cube.transform.position, playerPos);
            
            // Move towards player if within detection range and flashlight is NOT on them
            if (distance <= detectionRange && !IsFlashlightOnCube(cube))
            {
                Vector3 direction = (playerPos - cube.transform.position).normalized;
                cube.transform.position += direction * cubeSpeed * Time.deltaTime;
            }
        }
    }
    
    bool IsFlashlightOnCube(GameObject cube)
    {
        if (flashlightController == null || !flashlightController.IsFlashlightOn()) 
            return false;
        
        return flashlightController.IsHittingObject(cube);
    }
    
    IEnumerator DestroyCubeAfterDelay(GameObject cube)
    {
        // Prevent multiple destroy calls on same cube
        if (!activeCubes.Contains(cube)) yield break;
        
        activeCubes.Remove(cube);
        
        yield return new WaitForSeconds(destroyDelay);
        
        if (cube != null)
        {
            // Spawn particles
            if (destroyParticles != null)
            {
                ParticleSystem particles = Instantiate(destroyParticles, cube.transform.position, Quaternion.identity);
                particles.Play();
            }
            
            // Play destroy sound
            if (destroySound != null)
                destroySound.Play();
            
            Destroy(cube);
        }
    }
    
    void CheckForFlareInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CreateFlare();
        }
    }
    
    void CreateFlare()
    {
        Vector3 flarePosition = playerCamera.transform.position + playerCamera.transform.forward * 2f;
        
        GameObject flare = null;
        if (flarePrefab != null)
        {
            flare = Instantiate(flarePrefab, flarePosition, Quaternion.identity);
        }
        else
        {
            // Create simple flare if no prefab provided
            flare = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flare.transform.position = flarePosition;
            flare.transform.localScale = Vector3.one * 0.2f;
        }
        
        // Add light to flare
        Light flareLight = flare.AddComponent<Light>();
        flareLight.type = LightType.Point;
        flareLight.intensity = 0f;
        flareLight.range = 20f;
        flareLight.color = Color.white;
        
        StartCoroutine(FlareSequence(flareLight));
    }
    
    IEnumerator FlareSequence(Light flareLight)
    {
        // Gradually increase brightness
        float currentIntensity = 0f;
        
        while (currentIntensity < flareIntensity)
        {
            currentIntensity += Time.deltaTime * flareBrightnessSpeed;
            flareLight.intensity = currentIntensity;
            yield return null;
        }
        
        // Check if scene should become white and change level
        float totalLightIntensity = CalculateTotalSceneBrightness();
        
        if (totalLightIntensity > 10f) // Adjust threshold as needed
        {
            StartCoroutine(WhiteoutAndChangeLevel());
        }
    }
    
    float CalculateTotalSceneBrightness()
    {
        float total = 0f;
        
        Light[] allLights = FindObjectsOfType<Light>();
        foreach (Light light in allLights)
        {
            if (light.enabled)
                total += light.intensity;
        }
        
        return total;
    }
    
    IEnumerator WhiteoutAndChangeLevel()
    {
        // Create white overlay
        GameObject whiteOverlay = new GameObject("WhiteOverlay");
        Canvas canvas = whiteOverlay.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        
        UnityEngine.UI.Image whiteImage = whiteOverlay.AddComponent<UnityEngine.UI.Image>();
        whiteImage.color = new Color(1, 1, 1, 0);
        
        // Fade to white
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * 2f;
            whiteImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Load next level
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
    }
}
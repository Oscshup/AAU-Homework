using UnityEngine;
using System.Collections;

public class FlashLightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlight;
    public Transform flashlightTransform;
    public LayerMask raycastLayers = -1;
    
    [Header("Flashlight Properties")]
    public float range = 15f;
    public float intensity = 5f;
    public float spotAngle = 45f;
    public Color lightColor = Color.white;
    
    [Header("Battery System")]
    public bool useBattery = true;
    public float maxBatteryLife = 120f; // 2 minutes
    public float batteryDrainRate = 1f;
    public float lowBatteryThreshold = 0.2f;
    public AnimationCurve batteryFlickerCurve;
    
    [Header("Audio")]
    public AudioSource flashlightAudio;
    public AudioClip clickOnSound;
    public AudioClip clickOffSound;
    public AudioClip lowBatterySound;
    
    [Header("Visual Effects")]
    public GameObject flashlightModel;
    public Renderer flashlightRenderer;
    public Material onMaterial;
    public Material offMaterial;
    
    // Private variables
    private Camera playerCamera;
    private FPSController playerController;
    private bool isFlashlightOn = false;
    private float currentBattery;
    private float originalIntensity;
    private bool isLowBattery = false;
    private Coroutine batteryFlickerCoroutine;
    
    // Raycast hit info
    private RaycastHit currentHit;
    private bool isHittingSomething = false;

    void Start()
    {
        playerController = FindObjectOfType<FPSController>();
        playerCamera = playerController.GetPlayerCamera();
        
        SetupFlashlight();
        InitializeBattery();
    }
    
    void SetupFlashlight()
    {
        // Create flashlight if not assigned
        if (flashlight == null)
        {
            GameObject flashlightGO = new GameObject("Flashlight");
            flashlightGO.transform.SetParent(playerCamera.transform);
            flashlightGO.transform.localPosition = new Vector3(0.3f, -0.2f, 0.5f);
            
            flashlight = flashlightGO.AddComponent<Light>();
            flashlightTransform = flashlightGO.transform;
        }
        
        // Configure flashlight properties
        flashlight.type = LightType.Spot;
        flashlight.range = range;
        flashlight.intensity = intensity;
        flashlight.spotAngle = spotAngle;
        flashlight.color = lightColor;
        flashlight.enabled = false;
        
        originalIntensity = intensity;
        
        // Setup flashlight model visual
        if (flashlightRenderer != null && offMaterial != null)
        {
            flashlightRenderer.material = offMaterial;
        }
    }
    
    void InitializeBattery()
    {
        currentBattery = maxBatteryLife;
        
        if (batteryFlickerCurve.keys.Length == 0)
        {
            // Create default flicker curve if none provided
            batteryFlickerCurve = new AnimationCurve();
            batteryFlickerCurve.AddKey(0f, 1f);
            batteryFlickerCurve.AddKey(0.5f, 0.3f);
            batteryFlickerCurve.AddKey(1f, 1f);
        }
    }

    void Update()
    {
        HandleFlashlightInput();
        UpdateBattery();
        PerformRaycast();
        UpdateFlashlightDirection();
    }
    
    void HandleFlashlightInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }
    
    public void ToggleFlashlight()
    {
        if (useBattery && currentBattery <= 0)
        {
            // Can't turn on if battery is dead
            if (!isFlashlightOn)
            {
                PlaySound(lowBatterySound);
                return;
            }
        }
        
        isFlashlightOn = !isFlashlightOn;
        flashlight.enabled = isFlashlightOn;
        
        // Play audio
        PlaySound(isFlashlightOn ? clickOnSound : clickOffSound);
        
        // Update visual
        UpdateFlashlightVisual();
        
        // Stop/start battery flicker coroutine
        if (batteryFlickerCoroutine != null)
        {
            StopCoroutine(batteryFlickerCoroutine);
            batteryFlickerCoroutine = null;
        }
        
        if (isFlashlightOn && isLowBattery)
        {
            batteryFlickerCoroutine = StartCoroutine(BatteryFlicker());
        }
    }
    
    void UpdateBattery()
    {
        if (!useBattery) return;
        
        if (isFlashlightOn && currentBattery > 0)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
            currentBattery = Mathf.Max(0, currentBattery);
            
            // Check for low battery
            float batteryPercentage = currentBattery / maxBatteryLife;
            
            if (batteryPercentage <= lowBatteryThreshold && !isLowBattery)
            {
                isLowBattery = true;
                PlaySound(lowBatterySound);
                
                if (isFlashlightOn)
                {
                    batteryFlickerCoroutine = StartCoroutine(BatteryFlicker());
                }
            }
            
            // Turn off if battery dies
            if (currentBattery <= 0 && isFlashlightOn)
            {
                ToggleFlashlight();
            }
        }
    }
    
    IEnumerator BatteryFlicker()
    {
        float flickerTime = 0f;
        float flickerDuration = 2f;
        
        while (isFlashlightOn && isLowBattery && currentBattery > 0)
        {
            flickerTime += Time.deltaTime;
            
            // Use curve to control flicker intensity
            float curveTime = (flickerTime % flickerDuration) / flickerDuration;
            float flickerMultiplier = batteryFlickerCurve.Evaluate(curveTime);
            
            flashlight.intensity = originalIntensity * flickerMultiplier;
            
            yield return null;
        }
        
        // Restore original intensity when done flickering
        if (flashlight != null)
        {
            flashlight.intensity = originalIntensity;
        }
    }
    
    void UpdateFlashlightDirection()
    {
        if (flashlightTransform != null && playerCamera != null)
        {
            // Make flashlight follow camera direction
            flashlightTransform.rotation = playerCamera.transform.rotation;
        }
    }
    
    void PerformRaycast()
    {
        if (!isFlashlightOn || flashlight == null) 
        {
            isHittingSomething = false;
            return;
        }
        
        Vector3 rayOrigin = flashlight.transform.position;
        Vector3 rayDirection = flashlight.transform.forward;
        
        if (Physics.Raycast(rayOrigin, rayDirection, out currentHit, range, raycastLayers))
        {
            isHittingSomething = true;
            
            // Debug ray in scene view
            Debug.DrawRay(rayOrigin, rayDirection * currentHit.distance, Color.yellow);
        }
        else
        {
            isHittingSomething = false;
            Debug.DrawRay(rayOrigin, rayDirection * range, Color.red);
        }
    }
    
    void UpdateFlashlightVisual()
    {
        if (flashlightRenderer == null) return;
        
        if (isFlashlightOn && onMaterial != null)
        {
            flashlightRenderer.material = onMaterial;
        }
        else if (offMaterial != null)
        {
            flashlightRenderer.material = offMaterial;
        }
    }
    
    void PlaySound(AudioClip clip)
    {
        if (flashlightAudio != null && clip != null)
        {
            flashlightAudio.PlayOneShot(clip);
        }
    }
    
    // Public methods for other scripts
    public bool IsFlashlightOn()
    {
        return isFlashlightOn;
    }
    
    public bool IsHittingObject(GameObject target)
    {
        return isHittingSomething && currentHit.collider.gameObject == target;
    }
    
    public RaycastHit GetCurrentHit()
    {
        return currentHit;
    }
    
    public float GetBatteryPercentage()
    {
        if (!useBattery) return 1f;
        return currentBattery / maxBatteryLife;
    }
    
    public Vector3 GetFlashlightDirection()
    {
        return flashlight != null ? flashlight.transform.forward : Vector3.forward;
    }
    
    public Vector3 GetFlashlightPosition()
    {
        return flashlight != null ? flashlight.transform.position : Vector3.zero;
    }
    
    // Method to recharge battery (for pickups, etc.)
    public void RechargeBattery(float amount)
    {
        if (!useBattery) return;
        
        currentBattery = Mathf.Min(maxBatteryLife, currentBattery + amount);
        
        // Reset low battery state if recharged enough
        if (currentBattery > maxBatteryLife * lowBatteryThreshold)
        {
            isLowBattery = false;
            
            if (batteryFlickerCoroutine != null)
            {
                StopCoroutine(batteryFlickerCoroutine);
                batteryFlickerCoroutine = null;
                flashlight.intensity = originalIntensity;
            }
        }
    }
}
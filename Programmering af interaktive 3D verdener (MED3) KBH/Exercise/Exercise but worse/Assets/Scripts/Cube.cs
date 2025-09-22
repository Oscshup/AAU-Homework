using UnityEngine;

public class Cube : MonoBehaviour
{
    [Header("Cube Properties")]
    public float rotationSpeed = 30f;
    public Color normalColor = Color.gray;
    public Color threatenedColor = Color.red;
    public AudioSource ambientSound;
    
    private Renderer cubeRenderer;
    private bool isBeingDestroyed = false;
    private HorrorGameManager gameManager;
    
    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        gameManager = FindObjectOfType<HorrorGameManager>();
        
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = normalColor;
            // Make it emit light slightly
            cubeRenderer.material.EnableKeyword("_EMISSION");
            cubeRenderer.material.SetColor("_EmissionColor", normalColor * 0.3f);
        }
        
        // Add some random rotation
        transform.rotation = Random.rotation;
    }
    
    void Update()
    {
        if (isBeingDestroyed) return;
        
        // Slowly rotate the cube
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * rotationSpeed * 0.5f * Time.deltaTime);
        
        // Check if flashlight is pointing at this cube
        bool isFlashlightOn = IsFlashlightPointingAtMe();
        
        // Change color based on threat level
        if (cubeRenderer != null)
        {
            Color targetColor = isFlashlightOn ? threatenedColor : normalColor;
            cubeRenderer.material.color = Color.Lerp(cubeRenderer.material.color, targetColor, Time.deltaTime * 3f);
            cubeRenderer.material.SetColor("_EmissionColor", targetColor * 0.5f);
        }
        
        // Adjust ambient sound volume
        if (ambientSound != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Camera.main.transform.position);
            float volume = Mathf.Clamp01(1f - (distanceToPlayer / 20f));
            ambientSound.volume = volume * (isFlashlightOn ? 0.1f : 1f);
        }
    }
    
    bool IsFlashlightPointingAtMe()
    {
        FlashLightController flashlightController = FindObjectOfType<FlashLightController>();
        if (flashlightController == null) return false;
        
        return flashlightController.IsHittingObject(gameObject);
    }
    
    public void StartDestruction()
    {
        if (isBeingDestroyed) return;
        isBeingDestroyed = true;
        
        // Add some dramatic effect before destruction
        StartCoroutine(DestructionEffect());
    }
    
    System.Collections.IEnumerator DestructionEffect()
    {
        float timer = 0f;
        Vector3 originalScale = transform.localScale;
        
        while (timer < gameManager.destroyDelay)
        {
            timer += Time.deltaTime;
            
            // Shake and shrink
            float shake = Mathf.Sin(timer * 20f) * 0.1f;
            transform.position += Random.insideUnitSphere * shake;
            
            float shrinkFactor = 1f - (timer / gameManager.destroyDelay) * 0.5f;
            transform.localScale = originalScale * shrinkFactor;
            
            // Flicker color
            if (cubeRenderer != null)
            {
                Color flickerColor = Color.Lerp(threatenedColor, Color.white, Mathf.Sin(timer * 30f));
                cubeRenderer.material.color = flickerColor;
                cubeRenderer.material.SetColor("_EmissionColor", flickerColor);
            }
            
            yield return null;
        }
        
        // The actual destruction is handled by the HorrorGameManager
    }
    
    void OnTriggerEnter(Collider other)
    {
        // If cube touches player, trigger game over
        if (other.CompareTag("Player") || other.GetComponent<FPSController>() != null)
        {
            Debug.Log("Cube touched player! Game Over!");
            
            // Find and trigger game over UI
            HorrorGameUI gameUI = FindObjectOfType<HorrorGameUI>();
            if (gameUI != null)
            {
                gameUI.ShowGameOverMessage();
            }
            
            // Stop cube movement
            isBeingDestroyed = true;
        }
    }
}
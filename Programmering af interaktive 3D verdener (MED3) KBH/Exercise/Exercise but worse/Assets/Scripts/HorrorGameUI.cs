using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HorrorGameUI : MonoBehaviour
{
    [Header("UI References")]
    public Canvas gameUI;
    public Slider batterySlider;
    public TextMeshProUGUI batteryText;
    public TextMeshProUGUI cubeCountText;
    public TextMeshProUGUI instructionsText;
    public Image flashlightIcon;
    public Image lowBatteryWarning;
    
    [Header("Colors")]
    public Color fullBatteryColor = Color.green;
    public Color lowBatteryColor = Color.red;
    public Color flashlightOnColor = Color.yellow;
    public Color flashlightOffColor = Color.gray;
    
    [Header("Instructions")]
    [TextArea(3, 6)]
    public string gameInstructions = "WASD - Move\nShift - Run\nF - Toggle Flashlight\nMouse - Look Around\nESC - Toggle Cursor\n\nSurvive the cubes! Use your flashlight to destroy them before they reach you.";
    
    private FlashLightController flashlightController;
    private HorrorGameManager gameManager;
    private bool showInstructions = true;
    
    void Start()
    {
        flashlightController = FindObjectOfType<FlashLightController>();
        gameManager = FindObjectOfType<HorrorGameManager>();
        
        SetupUI();
        
        // Show instructions for a few seconds
        if (instructionsText != null)
        {
            instructionsText.text = gameInstructions;
            Invoke(nameof(HideInstructions), 8f);
        }
    }
    
    void SetupUI()
    {
        // Create UI if not already present
        if (gameUI == null)
        {
            CreateGameUI();
        }
        
        // Initialize battery slider
        if (batterySlider != null)
        {
            batterySlider.minValue = 0f;
            batterySlider.maxValue = 1f;
            batterySlider.value = 1f;
        }
        
        // Initialize low battery warning (hidden by default)
        if (lowBatteryWarning != null)
        {
            lowBatteryWarning.gameObject.SetActive(false);
        }
    }
    
    void CreateGameUI()
    {
        // This method would create UI programmatically if needed
        // For now, assume UI is set up in the scene
        Debug.Log("UI should be set up in the scene with proper references");
    }
    
    void Update()
    {
        UpdateBatteryDisplay();
        UpdateCubeCount();
        UpdateFlashlightIcon();
        HandleUIInput();
    }
    
    void UpdateBatteryDisplay()
    {
        if (flashlightController == null) return;
        
        float batteryPercentage = flashlightController.GetBatteryPercentage();
        
        // Update slider
        if (batterySlider != null)
        {
            batterySlider.value = batteryPercentage;
            
            // Change color based on battery level
            Image sliderFill = batterySlider.fillRect.GetComponent<Image>();
            if (sliderFill != null)
            {
                sliderFill.color = Color.Lerp(lowBatteryColor, fullBatteryColor, batteryPercentage);
            }
        }
        
        // Update text
        if (batteryText != null)
        {
            int batteryPercent = Mathf.RoundToInt(batteryPercentage * 100);
            batteryText.text = $"Battery: {batteryPercent}%";
            batteryText.color = batteryPercentage > 0.2f ? Color.white : lowBatteryColor;
        }
        
        // Show/hide low battery warning
        if (lowBatteryWarning != null)
        {
            bool shouldShowWarning = batteryPercentage <= 0.2f && batteryPercentage > 0;
            lowBatteryWarning.gameObject.SetActive(shouldShowWarning);
            
            if (shouldShowWarning)
            {
                // Pulsing effect
                float alpha = 0.5f + 0.5f * Mathf.Sin(Time.time * 5f);
                Color warningColor = lowBatteryWarning.color;
                warningColor.a = alpha;
                lowBatteryWarning.color = warningColor;
            }
        }
    }
    
    void UpdateCubeCount()
    {
        if (gameManager == null || cubeCountText == null) return;
        
        // Count remaining active cubes
        int activeCubeCount = 0;
        foreach (GameObject cube in gameManager.cubes)
        {
            if (cube != null && cube.activeInHierarchy)
                activeCubeCount++;
        }
        
        cubeCountText.text = $"Cubes Remaining: {activeCubeCount}";
        
        // Change color if getting close to winning
        if (activeCubeCount <= 3)
        {
            cubeCountText.color = Color.green;
        }
        else if (activeCubeCount <= 5)
        {
            cubeCountText.color = Color.yellow;
        }
        else
        {
            cubeCountText.color = Color.white;
        }
        
        // Victory condition
        if (activeCubeCount <= 0)
        {
            ShowVictoryMessage();
        }
    }
    
    void UpdateFlashlightIcon()
    {
        if (flashlightController == null || flashlightIcon == null) return;
        
        bool isFlashlightOn = flashlightController.IsFlashlightOn();
        flashlightIcon.color = isFlashlightOn ? flashlightOnColor : flashlightOffColor;
        
        // Add pulsing effect when on
        if (isFlashlightOn)
        {
            float pulse = 0.8f + 0.2f * Mathf.Sin(Time.time * 3f);
            Color iconColor = flashlightOnColor;
            iconColor.a = pulse;
            flashlightIcon.color = iconColor;
        }
    }
    
    void HandleUIInput()
    {
        // Toggle instructions
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (instructionsText != null)
            {
                showInstructions = !showInstructions;
                instructionsText.gameObject.SetActive(showInstructions);
            }
        }
        
        // Quick battery check
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (flashlightController != null)
            {
                float battery = flashlightController.GetBatteryPercentage();
                Debug.Log($"Battery: {Mathf.RoundToInt(battery * 100)}%");
            }
        }
    }
    
    void HideInstructions()
    {
        if (instructionsText != null)
        {
            instructionsText.gameObject.SetActive(false);
            showInstructions = false;
        }
    }
    
    void ShowVictoryMessage()
    {
        // Create victory UI
        GameObject victoryPanel = new GameObject("Victory Panel");
        victoryPanel.transform.SetParent(gameUI.transform);
        
        Canvas victoryCanvas = victoryPanel.AddComponent<Canvas>();
        victoryCanvas.overrideSorting = true;
        victoryCanvas.sortingOrder = 100;
        
        // Add background
        Image background = victoryPanel.AddComponent<Image>();
        background.color = new Color(0, 0, 0, 0.8f);
        
        RectTransform panelRect = victoryPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Add victory text
        GameObject textObj = new GameObject("Victory Text");
        textObj.transform.SetParent(victoryPanel.transform);
        
        TextMeshProUGUI victoryText = textObj.AddComponent<TextMeshProUGUI>();
        victoryText.text = "VICTORY!\n\nYou survived the cubes!\n\nPress G for Flare Exit or ESC to quit";
        victoryText.fontSize = 36;
        victoryText.color = Color.white;
        victoryText.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
    }
    
    public void ShowGameOverMessage()
    {
        // Create game over UI
        GameObject gameOverPanel = new GameObject("Game Over Panel");
        gameOverPanel.transform.SetParent(gameUI.transform);
        
        Canvas gameOverCanvas = gameOverPanel.AddComponent<Canvas>();
        gameOverCanvas.overrideSorting = true;
        gameOverCanvas.sortingOrder = 100;
        
        // Add background
        Image background = gameOverPanel.AddComponent<Image>();
        background.color = new Color(0.5f, 0, 0, 0.9f); // Dark red
        
        RectTransform panelRect = gameOverPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Add game over text
        GameObject textObj = new GameObject("Game Over Text");
        textObj.transform.SetParent(gameOverPanel.transform);
        
        TextMeshProUGUI gameOverText = textObj.AddComponent<TextMeshProUGUI>();
        gameOverText.text = "GAME OVER\n\nThe cubes got you!\n\nPress R to restart or ESC to quit";
        gameOverText.fontSize = 36;
        gameOverText.color = Color.red;
        gameOverText.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        // Freeze game
        Time.timeScale = 0f;
        
        // Handle restart input
        StartCoroutine(WaitForRestartInput());
    }
    
    System.Collections.IEnumerator WaitForRestartInput()
    {
        while (Time.timeScale == 0f)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                yield break;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1f;
                Application.Quit();
                yield break;
            }
            
            yield return null;
        }
    }
    
    // Public methods for other scripts
    public void UpdateBatterySlider(float value)
    {
        if (batterySlider != null)
        {
            batterySlider.value = value;
        }
    }
    
    public void ShowMessage(string message, float duration = 3f)
    {
        // Simple message display
        if (instructionsText != null)
        {
            instructionsText.text = message;
            instructionsText.gameObject.SetActive(true);
            
            CancelInvoke(nameof(HideInstructions));
            Invoke(nameof(HideInstructions), duration);
        }
    }
}
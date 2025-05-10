using UnityEngine;
using UnityEngine.UI;
// For CPU usage

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;       // Assign the main UI Panel in the inspector
    public KeyCode toggleKey = KeyCode.Tab; // Shortcut key to show/hide UI

    private bool isVisible = true;
    public Text statsText;

    private float deltaTime = 0.0f;
    private float updateInterval = 0.5f;
    private float timeSinceLastUpdate = 0.0f;


    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleUI();
        }
       if(isVisible)ShowFps();

    }

    public void ToggleUI()
    {
        isVisible = !isVisible;
        uiPanel.SetActive(isVisible);
    }
    public void ShowFps(){
         deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        timeSinceLastUpdate += Time.unscaledDeltaTime;

        if (timeSinceLastUpdate >= updateInterval )
        {
            float fps = 1.0f / deltaTime;
            float cpuFrameTime = Time.deltaTime * 100f; // in milliseconds

           statsText.text = $"FPS: {Mathf.Ceil(fps)}\nCPU Time: {cpuFrameTime:F1} ms";
             timeSinceLastUpdate = 0.0f;
        }
    }
}

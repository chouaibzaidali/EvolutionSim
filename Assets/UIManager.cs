using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;       // Assign the main UI Panel in the inspector
    public KeyCode toggleKey = KeyCode.Tab; // Shortcut key to show/hide UI

    private bool isVisible = true;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleUI();
        }
    }

    public void ToggleUI()
    {
        isVisible = !isVisible;
        uiPanel.SetActive(isVisible);
    }
}

using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private static string objectiveText;
    private static string objectiveText2;
    private LevelEnding levelEnding;
    private bool objectiveUpdated = false; // Flag to prevent multiple objective updates

    public TextMeshProUGUI objectiveTextMesh;
    // Reference to the canvas group that controls the objective UI visibility
    public CanvasGroup objectiveBackgroundCanvasGroup;
    public float displayDuration = 4f; // How long to display objective messages
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("ObjectiveManager Start method called");
        objectiveText = "Collect the key to unlock the door.";
        objectiveText2 = "Find the exit.";
        
        // Find the LevelEnding script in the scene instead of creating a new one
        levelEnding = FindAnyObjectByType<LevelEnding>();
        
        if (objectiveTextMesh != null)
        {
            objectiveTextMesh.text = objectiveText;
            Debug.Log("Set objective text to: " + objectiveText);
            
            // Show the objective initially
            if (objectiveBackgroundCanvasGroup != null)
            {
                ShowObjective(displayDuration);
            }
        }
        else
        {
            Debug.LogError("objectiveTextMesh is null! Make sure to assign it in the Inspector");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        // Check if the key was collected and update the objective
        if (levelEnding != null && levelEnding.IsKeyCollected() && !objectiveUpdated)
        {
            objectiveUpdated = true; // Set the flag to true to prevent further updates
            StartCoroutine(UpdateObjectiveAfterDelay());
        }
    }
    
    // Shows the objective UI for a specified duration
    public void ShowObjective(float duration)
    {
        if (objectiveBackgroundCanvasGroup != null)
        {
            objectiveBackgroundCanvasGroup.alpha = 1f;
            objectiveBackgroundCanvasGroup.interactable = true;
            objectiveBackgroundCanvasGroup.blocksRaycasts = true;
            
            // Hide after duration
            if (duration > 0)
            {
                Invoke("HideObjective", duration);
            }
        }
        else
        {
            Debug.LogError("objectiveBackgroundCanvasGroup is null! Make sure to assign it in the Inspector");
        }
    }
    
    // Hides the objective UI
    public void HideObjective()
    {
        if (objectiveBackgroundCanvasGroup != null)
        {
            objectiveBackgroundCanvasGroup.alpha = 0f;
            objectiveBackgroundCanvasGroup.interactable = false;
            objectiveBackgroundCanvasGroup.blocksRaycasts = false;
        }
    }
      private System.Collections.IEnumerator UpdateObjectiveAfterDelay()
    {
        if (objectiveTextMesh != null)
        {
            // Cancel any pending hide operations
            CancelInvoke("HideObjective");
            
            // First objective: Key collected
            objectiveTextMesh.text = "Key collected!";
            Debug.Log("Objective updated: Key collected!");
            
            // Show the UI manually instead of using ShowObjective()
            if (objectiveBackgroundCanvasGroup != null)
            {
                objectiveBackgroundCanvasGroup.alpha = 1f;
                objectiveBackgroundCanvasGroup.interactable = true;
                objectiveBackgroundCanvasGroup.blocksRaycasts = true;
            }
            
            // Wait for the first message duration
            yield return new WaitForSeconds(displayDuration);
            
            // Second objective: Find the exit
            objectiveTextMesh.text = objectiveText2;
            Debug.Log("Objective updated: " + objectiveText2);
            
            // No need to change UI visibility here, just show the new text
            // Wait for display duration and then hide
            yield return new WaitForSeconds(displayDuration);
            
            // Hide the objective UI
            HideObjective();
        }
        else
        {
            Debug.LogError("objectiveTextMesh is null in UpdateObjectiveAfterDelay!");
            yield return null;
        }
    }
}

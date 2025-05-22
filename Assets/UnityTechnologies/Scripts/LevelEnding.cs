using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;
    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;
    bool m_HasAudioPlayed;
    public float displayTime = 2f;
    private static bool keyCollected = false;

    public void setKeyFlag(bool value)
    {
        keyCollected = value;
        Debug.Log("Key collected: " + keyCollected);
    }
    public bool IsKeyCollected()
    {
        return keyCollected;
    }

    void OnTriggerEnter(Collider other)
    {
        if (keyCollected)
        {
            if (other.gameObject == player)
            {
                m_IsPlayerAtExit = true;
                Debug.Log("Player has reached the exit.");
            }
        }
    }
    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Start()
    {
        // Ensure the exit UI is hidden at the start
        if (exitBackgroundImageCanvasGroup != null)
        {
            exitBackgroundImageCanvasGroup.alpha = 0f;
            exitBackgroundImageCanvasGroup.interactable = false;
            exitBackgroundImageCanvasGroup.blocksRaycasts = false;
        }
        if (caughtBackgroundImageCanvasGroup != null)
        {
            caughtBackgroundImageCanvasGroup.alpha = 0f;
            caughtBackgroundImageCanvasGroup.interactable = false;
            caughtBackgroundImageCanvasGroup.blocksRaycasts = false;
        }

        // We don't need to manage the objective UI visibility here anymore
        // It's now handled by the ObjectiveManager
    }

    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, exitAudio);

        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, caughtAudio);

        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, AudioSource audioSource)
    {
        if (imageCanvasGroup != null)
        {
            imageCanvasGroup.alpha = Mathf.Clamp01(m_Timer / fadeDuration);
            imageCanvasGroup.interactable = true;
            imageCanvasGroup.blocksRaycasts = true;
        }


        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = Mathf.Clamp01(m_Timer / fadeDuration);
        if (m_Timer > fadeDuration + displayTime)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                // SceneManager.LoadScene(1);
                
                Debug.Log("Loading next level...");
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                // SceneManager.LoadScene(1);
                Debug.Log("Loading previous level...");
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                // SceneManager.LoadScene(1);

                Debug.Log("Loading previous level...");
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                // Restart the level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                // Display a message to the player
                Debug.Log("Press R to restart, Y to load next level, or Escape to quit.");
            }
            Debug.Log("Level Completed!");
        }
    }
}
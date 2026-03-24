using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Persist : MonoBehaviour
{
    [Header("If true: behaves normally. If false: this object will persist even on scene reset.")]
    public bool loseProgressOnSceneReset = true;

    private static Persist instance;
    private static string lastSceneName;

    void Awake()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (instance == null)
        {
            // First instance: make persistent
            instance = this;
            lastSceneName = currentScene;
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChanged;
            return;
        }

        // --- If we reach here, another instance exists ---
        if (instance != this)
        {
            if (loseProgressOnSceneReset)
            {
                // ORIGINAL BEHAVIOR:
                // If reloading same scene, replace the root
                if (currentScene == lastSceneName)
                {
                    StartCoroutine(ReplacePersistentRoot());
                }
                else
                {
                    Destroy(gameObject); // New scene; keep old instance
                }
            }
            else
            {
                // NEW BEHAVIOR:
                // Keep the first instance forever, discard all others
                Destroy(gameObject);
            }
        }
    }


    // --------------------------
    // Original replacement logic
    // --------------------------
    private IEnumerator ReplacePersistentRoot()
    {
        yield return null;

        if (instance != null)
        {
            SceneManager.activeSceneChanged -= instance.OnSceneChanged;
            Destroy(instance.gameObject);
        }

        instance = this;
        lastSceneName = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChanged;
    }


    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        lastSceneName = newScene.name;

        // Keep your special-case destruction logic:
        if (newScene.name == "Menu 1")
            Destroy(gameObject);
    }


    private void OnDestroy()
    {
        if (instance == this)
            SceneManager.activeSceneChanged -= OnSceneChanged;
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DepthPerception
{
    /// <summary>
    /// Minimal shared scene navigation for the title, gameplay, and ending flow.
    /// </summary>
    public sealed class SceneNavigator : MonoBehaviour
    {
        public const string MainMenuScene = "MainMenu";
        public const string Level01Scene = "Level01_Valley";
        public const string EndScreenScene = "EndScreen";

        public void StartGame()
        {
            LoadScene(Level01Scene);
        }

        public void ReturnToStart()
        {
            LoadScene(MainMenuScene);
        }

        public static void LoadScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("[SceneNavigator] Cannot load an empty scene name.");
                return;
            }

            Time.timeScale = 1f;
            Debug.Log($"[SceneNavigator] Loading {sceneName}");
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelTrancition : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}

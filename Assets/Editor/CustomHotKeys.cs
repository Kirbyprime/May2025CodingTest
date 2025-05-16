using UnityEditor;
using UnityEditor.SceneManagement;

public static class CustomHotKeys
{
    [MenuItem("Custom/Play %g")]
    private static void PlayStartupScene()
    {
        const string scenePath = "Assets/Scenes/Startup.unity";
        if (System.IO.File.Exists(scenePath)) 
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
                EditorApplication.isPlaying = true;
            }
        }
    }
}

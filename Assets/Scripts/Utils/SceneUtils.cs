using UnityEngine.SceneManagement;

public static class SceneUtils
{
    public enum GameScene
    {
        Startup,
        MainMenu,
        Gameplay
    }

    public static void LoadScene(GameScene scene)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.CloseAll();
        }
        SceneManager.LoadScene(scene.ToString());
    }
}
public class StartupManager : BaseUnityManager<StartupManager>
{
    protected override void Start()
    {
        base.Start();
        SceneUtils.LoadScene(SceneUtils.GameScene.MainMenu);
    }
}

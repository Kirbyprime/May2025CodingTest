public class MainMenuManager : BaseUnityManager<MainMenuManager>
{
    protected override void Awake()
    {
        base.Awake();

        MainMenuScreenDataProvider dataProvider = new MainMenuScreenDataProvider(null);
        UIManager.Instance.Open(dataProvider);
    }
}

public class GameplayManager : BaseUnityManager<GameplayManager>
{
    protected override void Awake()
    {
        base.Awake();
        GamePlayScreenDataProvider dataProvider = new GamePlayScreenDataProvider(null);
        UIManager.Instance.Open(dataProvider);
    }
}

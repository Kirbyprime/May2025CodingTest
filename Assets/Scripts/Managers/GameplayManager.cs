public class GameplayManager : BaseUnityManager<GameplayManager>
{
    protected override void Awake()
    {
        base.Awake();
        GamePlayScreenDataProvider dataProvider = new GamePlayScreenDataProvider(null)
        {
            GridSize = 5,
            NumRandomizations = 3
        };
        UIManager.Instance.Open(dataProvider);
    }
}

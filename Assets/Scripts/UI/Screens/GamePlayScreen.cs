using UnityEngine;
using UnityEngine.UI;

public class GamePlayScreen : ScreenBase<GamePlayScreen, GamePlayScreenDataProvider>
{
    [SerializeField] private Button _closeButton;

    public override void Initialize(GamePlayScreenDataProvider provider)
    {
        _closeButton.SafeAddClickListener(ExitGamePlay);
    }

    private void ExitGamePlay()
    {
        SceneUtils.LoadScene(SceneUtils.GameScene.MainMenu);
    }
}

public class GamePlayScreenDataProvider : ScreenDataProvider
{
    public GamePlayScreenDataProvider(ScreenParams data) : base(data)
    {
    }
}

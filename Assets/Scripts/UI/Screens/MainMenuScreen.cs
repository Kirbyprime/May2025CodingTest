using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : ScreenBase<MainMenuScreen, MainMenuScreenDataProvider>
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    public override void Initialize(MainMenuScreenDataProvider provider)
    {
        _startButton.SafeAddClickListener(OnStartClicked);
        _exitButton.SafeAddClickListener(OnExitClicked);
    }

    private void OnStartClicked()
    {
        SceneUtils.LoadScene(SceneUtils.GameScene.Gameplay);
    }

    private void OnExitClicked()
    {
        var popupParams = new ConfirmationPopupDataProvider.ConfirmationPopupParams()
        {
            Text = "Are you sure?",
            ButtonText = "Super Quit!",
            OnConfirm = QuitApplication

        };
        var popup = new ConfirmationPopupDataProvider(popupParams);
        UIManager.Instance.Open(popup);
    }

    private void QuitApplication()
    {
        UIManager.Instance.CloseAll();
        AssetUtils.ClearAllHandles();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

public class MainMenuScreenDataProvider : ScreenDataProvider
{
    public MainMenuScreenDataProvider(ScreenParams data) : base(data)
    {
    }
}

using UnityEngine;

public abstract class PopupBase : ScreenBase { }

public abstract class PopupBase<TPrefab, TProvider> : PopupBase where TPrefab : MonoBehaviour where TProvider : PopupDataProvider
{
    public new TProvider DataProvider => (TProvider)base.DataProvider;

    public override void Initialize(ScreenDataProvider provider)
    {
        base.DataProvider = provider;
        Initialize(DataProvider);
    }

    public abstract void Initialize(TProvider provider);
}

public abstract class PopupDataProvider : ScreenDataProvider
{
    public override UIManager.ScreenType ScreenType => UIManager.ScreenType.Popup;
    public override bool RequireBacker => true;

    public class PopupParams : ScreenParams { }

    protected PopupDataProvider(ScreenParams data) : base(data) { }
    protected override string GetPrefabPath() => $"{UIManager.PREFAB_BASE_POPUP_PATH}/{GetType().Name.Replace("DataProvider", ".prefab")}";
}

public abstract class PopupDataProvider<TPrefab> : PopupDataProvider where TPrefab : MonoBehaviour
{
    public PopupDataProvider() : base(null) {}
    public PopupDataProvider(ScreenParams data) : base(data) {}

}
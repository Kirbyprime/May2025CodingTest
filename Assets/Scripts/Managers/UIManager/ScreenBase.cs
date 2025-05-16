using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class ScreenBase : MonoBehaviour
{
    public ScreenDataProvider DataProvider { get; protected set; }
    public AsyncOperationHandle Handle { get; set; }

    public void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        OnHide();
    }

    public void Close() => UIManager.Instance.Close(this);

    public virtual void OnClose() { }
    public virtual void OnShow() { }
    public virtual void OnHide() { }

    public abstract void Initialize(ScreenDataProvider provider);
}

public abstract class ScreenBase<TPrefab, TProvider> : ScreenBase where TPrefab : MonoBehaviour where TProvider : ScreenDataProvider
{
    public new TProvider DataProvider => (TProvider)base.DataProvider;

    public override void Initialize(ScreenDataProvider provider)
    {
        base.DataProvider = provider;
        Initialize(DataProvider);
    }

    public abstract void Initialize(TProvider provider);
}

public abstract class ScreenDataProvider
{
    public class ScreenParams 
    {
        public Action OnClose;
    }

    public virtual ScreenBase Screen { get; internal set; }
    public virtual UIManager.ScreenType ScreenType { get; } = UIManager.ScreenType.Screen;
    public string AddressablePath { get; protected set; }
    public ScreenParams Data { get; protected set; }
    public virtual bool RequireBacker { get; } = false;

    public ScreenDataProvider(ScreenParams data)
    {
        AddressablePath = GetPrefabPath();
        Data = data;
    }

    public bool IsValid => AddressablePath != null;


    protected virtual string GetPrefabPath() => $"{UIManager.PREFAB_BASE_SCREEN_PATH}/{GetType().Name.Replace("DataProvider", ".prefab")}";
}

public class ScreenDataProvider<TPrefab> : ScreenDataProvider where TPrefab : MonoBehaviour
{
    public ScreenDataProvider() : base(null) { }
    public ScreenDataProvider(ScreenParams data) : base(data) { }
}
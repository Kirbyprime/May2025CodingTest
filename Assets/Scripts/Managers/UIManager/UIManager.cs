using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UIManager : BaseUnityManager<UIManager>
{
    [Serializable]
    private class ScreenCanvasPair
    {
        public ScreenType Type;
        public Canvas Canvas;
    }

    public enum ScreenType
    {
        Screen,
        Popup
    }

    public const string PREFAB_BASE_POPUP_PATH = "Assets/Prefabs/UI/Popups";
    public const string PREFAB_BASE_SCREEN_PATH = "Assets/Prefabs/UI/Screens";

    [SerializeField] private List<ScreenCanvasPair> _canvases;

    [SerializeField] private GameObject _fadePrefab;
    
    private GameObject _fadeObj;

    public Dictionary<ScreenType, ScreenBase> Current { get; private set; } = new();
    public Dictionary<ScreenType, Stack<ScreenBase>> Queues { get; } = new();

    public ScreenBase Open<TProvider>(TProvider dataProvider) where TProvider : ScreenDataProvider
    {
        if (dataProvider == null || !dataProvider.IsValid)
        {
            return null;
        }

        if (Current.TryGetValue(dataProvider.ScreenType, out ScreenBase existing))
        {
            existing.Hide();
            if (!Queues.ContainsKey(dataProvider.ScreenType))
            {
                Queues[dataProvider.ScreenType] = new Stack<ScreenBase>();
            }
            Queues[dataProvider.ScreenType].Push(existing);
            Current.Remove(dataProvider.ScreenType);
        }

        var handle = Addressables.LoadAssetAsync<GameObject>(dataProvider.AddressablePath);
        handle.WaitForCompletion();
        
        if (GetCanvas(dataProvider.ScreenType) is Canvas screenCanvas && screenCanvas != null)
        {
            ScreenBase instance = AssetUtils.InstantiateAsset<ScreenBase>(dataProvider.AddressablePath, screenCanvas.transform, false);
            if (instance != null)
            {
                instance.Initialize(dataProvider);
                instance.Handle = handle;
                dataProvider.Screen = instance;

                Current[dataProvider.ScreenType] = instance;
                instance.Show();
                UpdateBackerState();

                return instance;
            }
        }
        else
        {
            // failed to open, show next one
            return TryShowNext(dataProvider.ScreenType);
        }

        return null;
    }

    public void Close(ScreenType type)
    {
        if (Current.TryGetValue(type, out ScreenBase existing))
        {
            Close(existing);
        }
    }

    public void Close(ScreenBase screen)
    {
        if (screen == null || screen.DataProvider == null) return;

        if (screen.Handle.IsValid())
        {
            Addressables.Release(screen.Handle);
        }

        if (Current.ContainsKey(screen.DataProvider.ScreenType))
        {
            screen.OnClose();
            screen.DataProvider.Data?.OnClose?.Invoke();
            Current.Remove(screen.DataProvider.ScreenType);
            Destroy(screen.gameObject);
            TryShowNext(screen.DataProvider.ScreenType);
        }
        else
        {
            // destroys the game object, but does not attempt to remove from the stack
            Destroy(screen.gameObject);
        }
        UpdateBackerState();
    }

    public void CloseAll()
    {
        foreach (ScreenType type in Enum.GetValues(typeof(ScreenType)))
        {
            CloseAll(type);
        }
    }

    public void CloseAll(ScreenType type)
    {
        if (Queues.ContainsKey(type) && Queues.Count > 0)
        { 
            while (Queues[type].Count > 0)
            {
                Close(Queues[type].Pop());
            }
        }
        Close(type);
        UpdateBackerState();
    }

    private ScreenBase TryShowNext(ScreenType type)
    {
        if (!Queues.ContainsKey(type)) return null;

        while (Queues[type].Count > 0)
        {
            Current[type] = Queues[type].Pop();
            if (Current[type] != null && Current[type].gameObject != null) // safety check, in case popup was closed while in queue
            {
                Current[type].Show();
                UpdateBackerState();
                return Current[type];
            }
        }
        UpdateBackerState();
        return null;
    }

    private void UpdateBackerState()
    {
        if (_fadeObj == null && GetCanvas(ScreenType.Popup) is Canvas popupCanvas && popupCanvas != null)
        {
            _fadeObj = Instantiate(_fadePrefab, popupCanvas.transform);
            _fadeObj.transform.SetAsFirstSibling();
        }

        if (_fadeObj != null)
        {
            Current.TryGetValue(ScreenType.Popup, out ScreenBase existing);
            _fadeObj.SetActive(existing != null && existing.DataProvider.RequireBacker);
        }
    }

    private Canvas GetCanvas(ScreenType type)
    {
        return _canvases.FirstOrDefault(c => c.Type == type)?.Canvas;
    }
}
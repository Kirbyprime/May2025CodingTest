using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AssetUtils
{
    private static readonly Dictionary<string, AsyncOperationHandle> _handles = new();

    public static void ClearAllHandles()
    {
        foreach (var handle in _handles.Values)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
        _handles.Clear();
    }

    public static bool InstantiateAsset(string assetReferenceId, Transform parent, bool cacheResult = true)
    {
        if (string.IsNullOrEmpty(assetReferenceId) || parent == null)
        {
            return false;
        }

        return InstantiateAsset<Object>(assetReferenceId, parent, cacheResult) != null;
    }

    public static T InstantiateAsset<T>(string assetReferenceId, Transform parent, bool cacheResult = true) where T : Object
    {
        if (string.IsNullOrEmpty(assetReferenceId) || parent == null)
        {
            return default;
        }

        object target = null;
        
        if (_handles.ContainsKey(assetReferenceId))
        {
            target = _handles[assetReferenceId].Result;
        }
        else
        {
            var handle = Addressables.LoadAssetAsync<Object>(assetReferenceId);
            handle.WaitForCompletion();
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (cacheResult)
                { 
                    _handles[assetReferenceId] = handle;
                }
                target = handle.Result;
            }
        }

        if (target != null)
        {
            if (typeof(Component).IsAssignableFrom(typeof(T)))
            {
                var go = Object.Instantiate(target as GameObject, parent);
                return go.GetComponent<T>();
            }
            else
            {
                return Object.Instantiate(target as T, parent);
            }
        }

        return default;
    }
}
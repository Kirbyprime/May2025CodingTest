using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class UIUtils
{
}

public static class MonobehaviourUtils
{
    public static bool SafeSetActive(this MonoBehaviour target, bool isActive)
    {
        if (target == null || target.gameObject == null) return false;
        target.gameObject.SetActive(isActive);
        return isActive;
    }
}

public static class ButtonUtils
{
    public static bool SafeAddClickListener(this Button target, UnityAction action)
    {
        if (target == null || target.gameObject == null) return false;
        target.onClick.AddListener(action);
        return true;
    }
}
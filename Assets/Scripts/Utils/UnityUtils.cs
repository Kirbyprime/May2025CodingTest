using TMPro;
using UnityEngine;

public class UnityUtils { }

public static class GameObjectUtils
{
    public static bool SafeSetActive(this GameObject target, bool enabled)
    {
        if (target != null)
        {
            target.SetActive(enabled);
            return enabled;
        }
        return false;
    }
}

public static class TMPUtils
{
    public static bool SafeSetText<T>(this TMP_Text target, T text)
    {
        if (target != null)
        {
            target.text = text?.ToString();
            return true;
        }
        return false;
    }
}

using TMPro;
using UnityEngine;

public class MovesCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public int Count { get; private set; }

    public void Reset()
    {
        Count = 0;
        UpdateText();
    }

    public void Increment()
    {
        Count++;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.SafeSetText($"Turns: {Count}");
    }
}

public static class MovesCounterUtils
{
    public static bool SafeIncrement(this MovesCounter target)
    {
        if (target == null || target.gameObject == null) return false;
        target.Increment();
        return true;
    }

    public static bool SafeReset(this MovesCounter target)
    {
        if (target == null || target.gameObject == null) return false;
        target.Reset();
        return true;
    }
}
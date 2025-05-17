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

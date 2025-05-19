using System.Collections;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private string _prefix;
    [SerializeField] private TMP_Text _textfield;

    private float _elapsed;
    private bool _isPlaying;

    private void Start()
    {
        UpdateText();
        StartCoroutine(UpdateTimer());
    }

    public void StartTimer()
    {
        if(_isPlaying) return;
        UpdateText();
        _isPlaying = true;
    }

    public void StopTimer()
    { 
        if(!_isPlaying) return;
        StopCoroutine(UpdateTimer());
        UpdateText();
        _isPlaying = false;
    }

    public void ResetTimer()
    { 
        // technically I dont' care if this is running or not, you could reset while it's running
        _elapsed = 0;
    }

    public override string ToString()
    {
        // https://www.reddit.com/r/Unity3D/comments/1bd2cmw/is_there_an_easier_way_to_put_a_0_before_the/
        // also includes an optimization suggestion to split min / sec into individual textfields. Not done here because doing so would also require
        // writing an anchoring system for textfields so that they connect to each other properly ( not necessary with leading zeroes but still )
        int min = Mathf.FloorToInt(_elapsed / 60f);
        int sec = Mathf.FloorToInt(_elapsed % 60f);
        return $"{min:00}:{sec:00}";
    }

    private IEnumerator UpdateTimer()
    {
        _elapsed += Time.deltaTime;
        UpdateText();
        yield return new WaitForSeconds(1.0f);
    }

    private void UpdateText()
    {
        int min = Mathf.FloorToInt(_elapsed / 60f);
        int sec = Mathf.FloorToInt(_elapsed % 60f);
        _textfield.SafeSetText(string.Format(_prefix, ToString()));
    }
}

public static class GameTimerUtils
{
    public static bool SafeStartTimer(this GameTimer target)
    {
        if (target == null || target.gameObject == null) return false;
        target.StartTimer();
        return true;
    }

    public static bool SafeStopTimer(this GameTimer target)
    {
        if (target == null || target.gameObject == null) return false;
        target.StopTimer();
        return true;
    }

    public static bool SafeResetTimer(this GameTimer target)
    {
        if (target == null || target.gameObject == null) return false;
        target.ResetTimer();
        return true;
    }
}
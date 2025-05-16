using UnityEngine;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private UIManager.ScreenType _screenType;

    public void OnClose()
    {
        if (UIManager.Instance != null)
        { 
            UIManager.Instance.Close(_screenType);
        }
    }
}

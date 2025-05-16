#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class BaseUnityManager<T> : MonoBehaviour where T : BaseUnityManager<T>
{
    [SerializeField] private bool _persistant = true;

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
            }
            return _instance;
        }
    }

    protected virtual void OnEnable()
    {
        Application.quitting += OnApplicationQuit;
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnEditorStateChanged;
#endif
    }

    protected virtual void OnDisable()
    {
        Application.quitting -= OnApplicationQuit;
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnEditorStateChanged;
#endif
    }

    protected virtual void OnApplicationQuit()
    { 
    }

    protected virtual void Start()
    { 
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            if (_persistant)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        else if(_persistant)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnEditorStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            OnApplicationQuit();
        }
    }
#endif
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{
    public event Action<Lamp> OnStateChanged;
    public event Action<Lamp> OnClicked;

    [SerializeField] private Button _button;
    [SerializeField] private GameObject _activeState;

    public bool State { get; private set; }
    
    private readonly List<Lamp> _neighbours = new();

    private void Start()
    {
        _button.SafeAddClickListener(OnClick);    
    }

    public void AddNeighbour(Lamp lamp)
    {
        if (lamp == null) return;

        // in actual practice, would want to add entire list at once, and also check for duplicates
        _neighbours.Add(lamp);
    }

    public void IsEnabled(bool enabled)
    {
        _button.SafeSetInteractable(enabled);
    }

    public void Flip(bool changeNeighbours = true, bool broadcastEvent = true)
    {
        SetState(!State, changeNeighbours, broadcastEvent);
    }

    public void SetState(bool state, bool changeNeighbours = true, bool broadcastEvent = true)
    {
        State = state;
        _activeState.SafeSetActive(State);
        if (changeNeighbours)
        {
            foreach (var lamp in _neighbours)
            {
                lamp.Flip(false, broadcastEvent);
            }
        }

        if (broadcastEvent)
        {
            OnStateChanged?.Invoke(this);
        }
    }

    private void OnClick()
    {
        OnClicked?.Invoke(this);
    }
}

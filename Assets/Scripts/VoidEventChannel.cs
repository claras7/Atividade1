using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "Scriptable Objects/VoidEventChannel")]
public class VoidEventChannel : ScriptableObject
{
    public event Action OnEventRaised;
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}

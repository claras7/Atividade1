using UnityEngine;
using System;

public class DoorEventChannel : MonoBehaviour
{
    public static Action<string> OnButtonPressed;

    // Método para disparar o evento
    public static void RaiseButtonPressed(string buttonID)
    {
        // Chama o evento
        OnButtonPressed?.Invoke(buttonID);
    }
}

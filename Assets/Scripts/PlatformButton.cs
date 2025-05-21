using UnityEngine;
using System;

public class PlatformButton : MonoBehaviour
{
    
    public string buttonID = "Button1";  // Defina um ID único para este botão

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Verifica se é o Player que entrou no trigger
        {
            // Dispara o evento de pressão do botão
            DoorEventChannel.RaiseButtonPressed(buttonID);
        }
    }
}

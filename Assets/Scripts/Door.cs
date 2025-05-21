using UnityEngine;

public class Door : MonoBehaviour
{
    public string buttonID = "Button1";  // O mesmo ID do botão para corresponder

    private void OnEnable()
    {
        // quando o objeto é ativado
        DoorEventChannel.OnButtonPressed += OnButtonPressed;
    }

    private void OnDisable()
    {
        // Remove a inscrição ao desativar o objeto
        DoorEventChannel.OnButtonPressed -= OnButtonPressed;
    }

    private void OnButtonPressed(string id)
    {
        if (id == buttonID)
        {
            // Desativa o objeto porta para "abrir"
            gameObject.SetActive(false);
        }
    }
}

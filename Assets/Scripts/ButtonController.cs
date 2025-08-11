using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Enviando")] 
    public VoidEventChannel circleColorEvent;

    public VoidEventChannel circleSpecificColorEvent;

    public void MudaCor()
    {
        circleColorEvent.RaiseEvent();
    }

    public void MudaCorEspecifica(Color corEspecifica)
    {
        circleSpecificColorEvent.RaiseEvent(corEspecifica);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int moedas = 0;
    
    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            moedas++;
            PlayerObserverManager.ChangedMoedas(moedas);
        }
    }
}

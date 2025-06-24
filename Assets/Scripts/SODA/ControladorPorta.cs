using UnityEngine;

public class ControladorPorta : MonoBehaviour
{
    private Animator animator;
    private bool estaAberta = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("portaAberta", false); // Começa fechada
    }

    public void AlternarPorta()
    {
        estaAberta = !estaAberta;
        animator.SetBool("portaAberta", estaAberta);
    }

    // Você também pode criar métodos separados se quiser:
    public void AbrirPorta()
    {
        estaAberta = true;
        animator.SetBool("portaAberta", true);
    }

    public void FecharPorta()
    {
        estaAberta = false;
        animator.SetBool("portaAberta", false);
    }
}
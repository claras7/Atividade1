using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaquinaDeRefrigerante : MonoBehaviour
{
    [Header("Referências UI")]
    public Animator animator;
    public GameObject portaPanel;
    public GameObject visorPanel;
    public TextMeshProUGUI visorTexto;
    public Transform estoque;
    public GameObject prefabLatinha;

    [Header("Botões")]
    public Button botaoInserir, botaoCancelar, botaoComprar, botaoManutencao;

    [Header("Estoque")]
    public int estoqueAtual = 0;

    [HideInInspector] public Estado estadoAtual;
    public Estado estadoManutencao, estadoSemRefrigerante, estadoSemMoeda, estadoComMoeda, estadoVenda;

    void Start()
    {
        estadoManutencao = new EstadoManutencao(this);
        estadoSemRefrigerante = new EstadoSemRefrigerante(this);
        estadoSemMoeda = new EstadoSemMoeda(this);
        estadoComMoeda = new EstadoComMoeda(this);
        estadoVenda = new EstadoVenda(this);

        if (estoqueAtual > 0)
            DefinirEstado(estadoSemMoeda);
        else
            DefinirEstado(estadoSemRefrigerante);

        botaoInserir.onClick.AddListener(() => InserirMoeda());
        botaoCancelar.onClick.AddListener(() => Cancelar());
        botaoComprar.onClick.AddListener(() => Comprar());
        botaoManutencao.onClick.AddListener(() => Manutencao());
    }

    public void DefinirEstado(Estado novoEstado)
    {
        estadoAtual = novoEstado;
        Debug.Log("Mudou para o estado: " + estadoAtual.GetType().Name);

        estadoAtual.Entrar();
        AtualizarAnimator();
        AtualizarBotoes();
    }

    public void InserirMoeda() => estadoAtual.InserirMoeda();
    public void Cancelar() => estadoAtual.Cancelar();
    public void Comprar() => estadoAtual.Comprar();
    public void Manutencao() => estadoAtual.Manutencao();

   public void AdicionarLatinha()
{
    Debug.Log("Adicionando latinha ao estoque");
    estoqueAtual++;
    GameObject nova = Instantiate(prefabLatinha, estoque);
    nova.transform.localPosition = new Vector3(0, estoque.childCount * -30, 0);
    AtualizarVisor($"Latinhas: {estoqueAtual}");
}


    public void RetirarLatinha()
    {
        if (estoqueAtual <= 0) return;

        estoqueAtual--;
        if (estoque.childCount > 0)
        {
            Destroy(estoque.GetChild(estoque.childCount - 1).gameObject);
        }
    }

    void AtualizarAnimator()
    {
        animator.SetBool("Manutencao", estadoAtual == estadoManutencao);
        animator.SetBool("SemRefrigerante", estadoAtual == estadoSemRefrigerante);
        animator.SetBool("SemMoeda", estadoAtual == estadoSemMoeda);
        animator.SetBool("ComMoeda", estadoAtual == estadoComMoeda);
        animator.SetBool("Venda", estadoAtual == estadoVenda);
    }

    void AtualizarBotoes()
    {
        if (estadoAtual == estadoManutencao)
        {
            botaoInserir.interactable = true;
            botaoCancelar.interactable = false;
            botaoComprar.interactable = false;
            botaoManutencao.interactable = true;
        }
        else if (estadoAtual == estadoSemRefrigerante)
        {
            botaoInserir.interactable = false;
            botaoCancelar.interactable = false;
            botaoComprar.interactable = false;
            botaoManutencao.interactable = true;
        }
        else if (estadoAtual == estadoSemMoeda)
        {
            botaoInserir.interactable = true;
            botaoCancelar.interactable = false;
            botaoComprar.interactable = false;
            botaoManutencao.interactable = true;
        }
        else if (estadoAtual == estadoComMoeda)
        {
            botaoInserir.interactable = false;
            botaoCancelar.interactable = true;
            botaoComprar.interactable = true;
            botaoManutencao.interactable = false;
        }
        else if (estadoAtual == estadoVenda)
        {
            botaoInserir.interactable = false;
            botaoCancelar.interactable = false;
            botaoComprar.interactable = false;
            botaoManutencao.interactable = false;
        }
    }

    public void AtualizarVisor(string texto)
    {
        visorTexto.text = texto;
    }
}

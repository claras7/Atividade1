using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MaquinaRefrigerante : MonoBehaviour
{
    [Header("Referências UI")]
    public Button BotaoInserir;
    public Button BotaoCancelar;
    public Button BotaoComprar;
    public Button BotaoManutencao;
    public TextMeshProUGUI TextoStatus;

    [HideInInspector] public int Estoque = 0;

    private IEstadoMaquina estadoAtual;

    private EstadoSemMoeda semMoeda;
    private EstadoComMoeda comMoeda;
    private EstadoManutencao manutencao;
    private EstadoVenda venda;
    private EstadoSemRefrigerante semRefrigerante;

    private void Start()
    {
        semMoeda = new EstadoSemMoeda(this);
        comMoeda = new EstadoComMoeda(this);
        manutencao = new EstadoManutencao(this);
        venda = new EstadoVenda(this);
        semRefrigerante = new EstadoSemRefrigerante(this);

        if (Estoque <= 0)
            MudarEstado(semRefrigerante);
        else
            MudarEstado(semMoeda);

        BotaoInserir.onClick.AddListener(() =>
        {
            if (estadoAtual == manutencao)
                AdicionarEstoque();
            else
                InserirMoeda();
        });

        BotaoCancelar.onClick.AddListener(Cancelar);
        BotaoComprar.onClick.AddListener(Comprar);
        BotaoManutencao.onClick.AddListener(ToggleManutencao);
    }

    private void Update()
    {
        estadoAtual?.Executar();
    }

    public void MudarEstado(IEstadoMaquina novoEstado)
    {
        estadoAtual?.Sair();
        estadoAtual = novoEstado;
        estadoAtual.Entrar();
        AtualizarTextoStatus();
    }

    void AtualizarTextoStatus()
    {
        if (estadoAtual == manutencao)
            TextoStatus.text = "Modo Manutenção";
        else if (estadoAtual == semRefrigerante)
            TextoStatus.text = "Sem Refrigerante";
        else if (estadoAtual == semMoeda)
            TextoStatus.text = "Insira uma moeda";
        else if (estadoAtual == comMoeda)
            TextoStatus.text = "Pronto para comprar";
        else if (estadoAtual == venda)
            TextoStatus.text = "Vendendo refrigerante...";
        else
            TextoStatus.text = "";
    }

    public void AdicionarEstoque()
    {
        Estoque++;
        Debug.Log("Estoque aumentado: " + Estoque);
    }

    public void InserirMoeda()
    {
        if (Estoque > 0)
            MudarEstado(comMoeda);
    }

    public void Cancelar()
    {
        if (estadoAtual == comMoeda)
            MudarEstado(semMoeda);
    }

    public void Comprar()
    {
        if (estadoAtual == comMoeda && Estoque > 0)
        {
            Estoque--;
            MudarEstado(venda);
            StartCoroutine(VoltarPosVenda());
        }
    }

    IEnumerator VoltarPosVenda()
    {
        yield return new WaitForSeconds(2f);
        if (Estoque > 0)
            MudarEstado(semMoeda);
        else
            MudarEstado(semRefrigerante);
    }

    public void ToggleManutencao()
    {
        if (estadoAtual == manutencao)
        {
            if (Estoque > 0)
                MudarEstado(semMoeda);
            else
                MudarEstado(semRefrigerante);
        }
        else if (estadoAtual != venda)
        {
            MudarEstado(manutencao);
        }
    }

    // Interface do estado
    private interface IEstadoMaquina
    {
        void Entrar();
        void Executar();
        void Sair();
    }

    private class EstadoSemMoeda : IEstadoMaquina
    {
        private MaquinaRefrigerante maquina;
        public EstadoSemMoeda(MaquinaRefrigerante maquina) { this.maquina = maquina; }
        public void Entrar()
        {
            Debug.Log("Entrou no estado Sem Moeda");
            maquina.BotaoInserir.interactable = true;
            maquina.BotaoCancelar.interactable = false;
            maquina.BotaoComprar.interactable = false;
            maquina.BotaoManutencao.interactable = true;
        }
        public void Executar() { }
        public void Sair() { Debug.Log("Saiu do estado Sem Moeda"); }
    }

    private class EstadoComMoeda : IEstadoMaquina
    {
        private MaquinaRefrigerante maquina;
        public EstadoComMoeda(MaquinaRefrigerante maquina) { this.maquina = maquina; }
        public void Entrar()
        {
            Debug.Log("Entrou no estado Com Moeda");
            maquina.BotaoInserir.interactable = false;
            maquina.BotaoCancelar.interactable = true;
            maquina.BotaoComprar.interactable = true;
            maquina.BotaoManutencao.interactable = false;
        }
        public void Executar() { }
        public void Sair() { Debug.Log("Saiu do estado Com Moeda"); }
    }

    private class EstadoManutencao : IEstadoMaquina
    {
        private MaquinaRefrigerante maquina;
        public EstadoManutencao(MaquinaRefrigerante maquina) { this.maquina = maquina; }
        public void Entrar()
        {
            Debug.Log("Entrou no modo Manutenção");
            maquina.BotaoInserir.interactable = true;
            maquina.BotaoCancelar.interactable = false;
            maquina.BotaoComprar.interactable = false;
            maquina.BotaoManutencao.interactable = true;
        }
        public void Executar() { }
        public void Sair() { Debug.Log("Saiu do modo Manutenção"); }
    }

    private class EstadoVenda : IEstadoMaquina
    {
        private MaquinaRefrigerante maquina;
        public EstadoVenda(MaquinaRefrigerante maquina) { this.maquina = maquina; }
        public void Entrar()
        {
            Debug.Log("Entrou no estado Venda");
            maquina.BotaoInserir.interactable = false;
            maquina.BotaoCancelar.interactable = false;
            maquina.BotaoComprar.interactable = false;
            maquina.BotaoManutencao.interactable = false;
        }
        public void Executar() { }
        public void Sair() { Debug.Log("Saiu do estado Venda"); }
    }

    private class EstadoSemRefrigerante : IEstadoMaquina
    {
        private MaquinaRefrigerante maquina;
        public EstadoSemRefrigerante(MaquinaRefrigerante maquina) { this.maquina = maquina; }
        public void Entrar()
        {
            Debug.Log("Entrou no estado Sem Refrigerante");
            maquina.BotaoInserir.interactable = false;
            maquina.BotaoCancelar.interactable = false;
            maquina.BotaoComprar.interactable = false;
            maquina.BotaoManutencao.interactable = true;
        }
        public void Executar() { }
        public void Sair() { Debug.Log("Saiu do estado Sem Refrigerante"); }
    }
}

using System.Collections;
using UnityEngine;

public class EstadoManutencao : Estado
{
    public EstadoManutencao(MaquinaDeRefrigerante maquina) : base(maquina) { }

    public override void Entrar()
    {
        maquina.portaPanel.SetActive(true);
        maquina.portaPanel.GetComponent<Animator>().SetTrigger("Abrir");
        maquina.AtualizarVisor($"Modo manutenção - Latinhas: {maquina.estoqueAtual}");
    }

    public override void InserirMoeda()
{
    Debug.Log("InserirMoeda chamado no EstadoManutencao");
    maquina.AdicionarLatinha();
}


    public override void Cancelar()
    {
        maquina.AtualizarVisor("Modo manutenção");
    }

    public override void Comprar()
    {
        maquina.AtualizarVisor("Modo manutenção");
    }

    public override void Manutencao()
    {
        maquina.portaPanel.GetComponent<Animator>().SetTrigger("Fechar");
        maquina.StartCoroutine(DesativarPortaAposFechar());
    }

    private IEnumerator DesativarPortaAposFechar()
    {
        yield return new WaitForSeconds(1f);
        maquina.portaPanel.SetActive(false);

        if (maquina.estoqueAtual > 0)
            maquina.DefinirEstado(maquina.estadoSemMoeda);
        else
            maquina.DefinirEstado(maquina.estadoSemRefrigerante);
    }
}
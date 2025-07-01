using System.Collections;
using UnityEngine;

public class EstadoVenda : Estado
{
    public EstadoVenda(MaquinaDeRefrigerante maquina) : base(maquina) { }

    public override void Entrar()
    {
        maquina.AtualizarVisor("Vendendo...");
        maquina.StartCoroutine(EntregarLatinhaCoroutine());
    }

    private IEnumerator EntregarLatinhaCoroutine()
    {
        maquina.RetirarLatinha();
        yield return new WaitForSeconds(2f);

        if (maquina.estoqueAtual > 0)
            maquina.DefinirEstado(maquina.estadoSemMoeda);
        else
            maquina.DefinirEstado(maquina.estadoSemRefrigerante);
    }

    public override void InserirMoeda() { }
    public override void Cancelar() { }
    public override void Comprar() { }
    public override void Manutencao() { }
}
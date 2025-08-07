using UnityEngine;

public class EstadoSemMoeda : Estado
{
    public EstadoSemMoeda(MaquinaDeRefrigerante maquina) : base(maquina) { }

    public override void Entrar()
    {
        maquina.AtualizarVisor("Insira");
    }

    public override void InserirMoeda()
    {
        maquina.DefinirEstado(maquina.estadoComMoeda);
    }

    public override void Cancelar()
    {
        maquina.AtualizarVisor("Sem moeda");
    }

    public override void Comprar()
    {
        maquina.AtualizarVisor("Insira primeiro");
    }

    public override void Manutencao()
    {
        maquina.DefinirEstado(maquina.estadoManutencao);
    }
}
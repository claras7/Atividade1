using UnityEngine;

public class EstadoComMoeda : Estado
{
    public EstadoComMoeda(MaquinaDeRefrigerante maquina) : base(maquina) { }

    public override void Entrar()
    {
        maquina.AtualizarVisor("OK");
    }

    public override void InserirMoeda()
    {
        maquina.AtualizarVisor("Já tem moeda!");
    }

    public override void Cancelar()
    {
        maquina.AtualizarVisor("Compra cancelada");
        maquina.DefinirEstado(maquina.estadoSemMoeda);
    }

    public override void Comprar()
    {
        maquina.DefinirEstado(maquina.estadoVenda);
    }

    public override void Manutencao()
    {
        maquina.AtualizarVisor("Impossível entrar em manutenção");
    }
}
using UnityEngine;

public class EstadoSemRefrigerante : Estado
{
    public EstadoSemRefrigerante(MaquinaDeRefrigerante maquina) : base(maquina) { }

    public override void Entrar()
    {
        maquina.AtualizarVisor("VAZIO");
    }

    public override void InserirMoeda()
    {
        maquina.AtualizarVisor("VAZIO");
    }

    public override void Cancelar()
    {
        maquina.AtualizarVisor("VAZIO");
    }

    public override void Comprar()
    {
        maquina.AtualizarVisor("VAZIO");
    }

    public override void Manutencao()
    {
        maquina.DefinirEstado(maquina.estadoManutencao);
    }
}
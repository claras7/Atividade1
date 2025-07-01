using UnityEngine;

public abstract class Estado
{
    protected MaquinaDeRefrigerante maquina;

    public Estado(MaquinaDeRefrigerante maquina)
    {
        this.maquina = maquina;
    }

    public virtual void Entrar() { }

    public abstract void InserirMoeda();
    public abstract void Cancelar();
    public abstract void Comprar();
    public abstract void Manutencao();
}
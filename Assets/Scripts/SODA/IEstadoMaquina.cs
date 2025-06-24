public interface IEstadoMaquina
{
    void Entrar();      // Ao entrar no estado
    void Executar();    // Executado todo frame
    void Sair();        // Ao sair do estado
}
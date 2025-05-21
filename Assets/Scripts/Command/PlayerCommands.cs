using UnityEngine;

public class MoveUp : ICommand
{
    private Transform playerTransform;

    public MoveUp(Transform transform)
    {
        playerTransform = transform;
    }
    public void Do()
    {
        playerTransform.position += Vector3.up;
    }

    public void Undo()
    {
        playerTransform.position -= Vector3.up;
    }
}

public class MoveRight : ICommand
{
    
    private Transform playerTransform;

    public MoveRight(Transform transform)
    {
        playerTransform = transform;
    }
    public void Do()
    {
        playerTransform.position += Vector3.right;
    }

    public void Undo()
    {
        playerTransform.position -= Vector3.right;
    }
}

public class GetCoin : ICommand
{
    private SimplePlayer player;
    private GameObject coin;

    public GetCoin(SimplePlayer player, GameObject coin)
    {
        this.player = player;
        this.coin = coin;
    }
    public void Do()
    {
        player.moedas++;
        coin.SetActive(false);
    }

    public void Undo()
    {
        player.moedas--;
        coin.SetActive(true);
        //lembrar de executar a função do Undo mais uma vez
        player.UndoLastCommand();
    }
}


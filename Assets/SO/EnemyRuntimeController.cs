using UnityEngine;

public class EnemyRuntimeController : MonoBehaviour
{
    public EnemySO enemyData;

    public string EnemyName=>enemyData.EnemyName;
    public int HP=>enemyData.HP;
    public int Damage => enemyData.Damage;
    public float Speed => enemyData.Speed;
    public Sprite EnemySprite => enemyData.EnemySprite;
    public Animator animator => enemyData.animator;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

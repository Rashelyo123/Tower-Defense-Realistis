
using Unity.VisualScripting;
using UnityEngine;

public class DeadState : IEnemyState
{
    public void Execute(Enemy enemy)
    {
        LevelManager.Instance.totalEnemies--;
        LevelManager.Instance.PlayerMoney += 10;
        enemy.destroyEnemy();

    }
}

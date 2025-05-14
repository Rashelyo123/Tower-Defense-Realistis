using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingState : IEnemyState
{
    public void Execute(Enemy enemy)
    {
        enemy.transform.Translate(Vector3.forward * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private IEnemyState currentState;

    void Start()
    {
        ChangeState(new MovingState());
    }
    public void ChangeState(IEnemyState newState)
    {
        currentState = newState;
    }

    void Update()
    {
        currentState.Execute(this);
    }
    public void destroyEnemy()
    {
        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        // Handle taking damage here
        // For example, reduce health and check if dead
        // If dead, change state to DeadState
        ChangeState(new DeadState());
    }
}

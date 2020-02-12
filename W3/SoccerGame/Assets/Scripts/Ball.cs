using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 initialPosition;

    public void Awake()
    {
        initialPosition = gameObject.transform.position;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Goal" && collision.name == "PlayerGoal")
        {
            print("Red Team Scored!");
            ServicesLocator.EventManager.Fire(new GoalScoredOnBlueTeam());
        }

        if (collision.tag == "Goal" && collision.name == "EnemyGoal")
        {
            print("Blue Team Scored!");
            ServicesLocator.EventManager.Fire(new GoalScoredOnRedTeam());
        }
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
    }
}

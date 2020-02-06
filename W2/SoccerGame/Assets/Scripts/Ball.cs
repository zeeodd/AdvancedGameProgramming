using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Goal")
        {
            print("Goal!");
            ServicesLocator.EventManager.Fire(new GoalScored(collision.name == "PlayerGoal"));
        }
    }

}

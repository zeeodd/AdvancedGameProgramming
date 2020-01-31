using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager
{
    public GameObject[] enemies;

    public void Initialize()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void MoveTowardBall()
    {
        foreach (GameObject enemy in ServicesLocator.AIManager.enemies)
        {
            enemy.GetComponent<AIController>().MoveTowardsBall();
        }
    }
}

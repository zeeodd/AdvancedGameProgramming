using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager
{
    public List<GameObject> enemies = new List<GameObject>();

    public void Initialize()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy);
        }
    }

    public void MoveTowardsBall()
    {
        if (enemies.Count == 0) Error.PrintError("There are no enemies left!");
        else
        {
            foreach (GameObject enemy in ServicesLocator.AIManager.enemies)
            {
                enemy.GetComponent<AIController>().MoveTowardsBall();
            }
        }
    }

    public void DestroyEnemy()
    {
        foreach (GameObject enemy in ServicesLocator.AIManager.enemies)
        {
            enemy.GetComponent<AIController>().DestroyEnemy();
            enemies.Remove(enemy);
        }
    }
}

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

    public void Update()
    {
        Debug.Log(GameObject.FindGameObjectsWithTag("Enemy"));
    }
}

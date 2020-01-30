using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Awake()
    {
        ServicesLocator.GameManager = this;
        ServicesLocator.AIManager = new AIManager();
    }

    public void Update()
    {
        print(ServicesLocator.AIManager.enemies);
    }
}

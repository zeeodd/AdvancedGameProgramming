using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Awake()
    {
        ServicesLocator.GameManager = this;
        ServicesLocator.AIManager = new AIManager();
        ServicesLocator.InputManager = new InputManager();
    }

    public void Start()
    {
        ServicesLocator.AIManager.Initialize();
        ServicesLocator.InputManager.Initialize();
    }

    public void Update()
    {
        ServicesLocator.AIManager.MoveTowardsBall();
        ServicesLocator.InputManager.MovePlayer();
    }
}

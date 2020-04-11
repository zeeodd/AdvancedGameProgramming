using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePower : MonoBehaviour
{
    public KeyCode activationKey;

    public float degreesPerSecond;

    public enum Direction { Left, Right }
    public Direction direction;

    /*
     * Private variables to control functionality
     */
    private bool gameHasStarted = false;
    private bool isRotating = false;
    private Quaternion restingRotation = new Quaternion(0f, 0f, 0f, 0f);
    private GameObject playerGameObject;

    private void Start()
    {
        RegisterListeners();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(activationKey) && gameHasStarted)
        {
            playerGameObject = ServicesLocator.UserPlayer[0]._gameObject;

            if (!isRotating) isRotating = true;
            else 
            {
                ResetRotation();
                isRotating = false;
            } 
        }

        if (isRotating)
        {
            playerGameObject.transform.Rotate(0f, 0f, degreesPerSecond);
        }
    }

    private void ResetRotation()
    {
        playerGameObject.transform.rotation = restingRotation;
    }

    private void InitializePowers(AGPEvent e)
    {
        gameHasStarted = true;
    }

    private void RegisterListeners()
    {
        ServicesLocator.EventManager.Register<GameStart>(InitializePowers);
    }

    private void UnregisterListeners()
    {
        ServicesLocator.EventManager.Unregister<GameStart>(InitializePowers);
    }
}

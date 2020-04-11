using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePower : MonoBehaviour
{
    [Header("Rotate Power Attributes")]
    public KeyCode activationKey;

    public float degreesPerFrame;

    public enum Direction { Left, Right }
    public Direction direction;

    public TrailRenderer effect;

    /*
     * Private variables to control functionality
     */
    private bool gameHasStarted = false;
    private bool isRotating = false;
    private bool effectAdded = false;
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

            if (!isRotating)
            {
                Invoke("InitEffect", 0f);
                isRotating = true;
            }
            else 
            {
                ResetRotation();
                Invoke("DestroyEffect", 0f);
                isRotating = false;
            } 
        }

        if (isRotating) Rotate();
    }

    private void ResetRotation()
    {
        playerGameObject.transform.rotation = restingRotation;
    }

    private void Rotate()
    {
        if (direction.Equals(Direction.Left)) playerGameObject.transform.Rotate(0f, 0f, degreesPerFrame);

        if (direction.Equals(Direction.Right)) playerGameObject.transform.Rotate(0f, 0f, -degreesPerFrame);
    }

    private void InitEffect()
    {
        if (!effectAdded)
        {
            GameObject trailEffect = Instantiate(Resources.Load<GameObject>("Prefabs/Trail"));
            trailEffect.gameObject.transform.parent = playerGameObject.transform;
            trailEffect.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            playerGameObject.transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
        }
    }

    private void DestroyEffect()
    {
        playerGameObject.transform.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
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

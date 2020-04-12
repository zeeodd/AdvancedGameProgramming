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

    public enum Effect { Particles, Trail }
    public Effect effect;

    [Header("Visual Effect")]
    public Gradient gradient;

    [Range(0f, 1f)]
    public float tailLength;

    /*
     * Private variables to control functionality
     */
    private bool gameHasStarted = false;
    private bool isRotating = false;
    private bool effectAdded = false;
    private Quaternion restingRotation = new Quaternion(0f, 0f, 0f, 0f);
    private GameObject playerGameObject;
    private GameObject currentEffect;

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
            switch(effect)
            {
                case Effect.Trail:
                    GameObject trailEffect = Instantiate(Resources.Load<GameObject>("Prefabs/Trail"));
                    currentEffect = trailEffect;
                    trailEffect.gameObject.transform.parent = playerGameObject.transform;
                    trailEffect.transform.localPosition = new Vector3(0f, 0f, 0f);
                    trailEffect.GetComponent<TrailRenderer>().colorGradient = gradient;
                    trailEffect.GetComponent<TrailRenderer>().time = tailLength;
                    break;
                case Effect.Particles:
                    GameObject particlesEffect = Instantiate(Resources.Load<GameObject>("Prefabs/Burst"));
                    currentEffect = particlesEffect;
                    particlesEffect.gameObject.transform.parent = playerGameObject.transform;
                    particlesEffect.transform.localPosition = new Vector3(0f, 0f, 0f);
                    StartCoroutine(IncreaseRadius(0.2f, 0.7f, 0.5f));
                    //particlesEffect.GetComponent<ParticleSystem>().colorOverLifetime = gradient;
                    break;
            }

            
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

    IEnumerator IncreaseRadius(float startRadius, float endRadius, float duration)
    {
        float t = 0f;
        var shape = currentEffect.GetComponent<ParticleSystem>().shape;
        shape.radius = startRadius;

        while (t < duration)
        {
            t += Time.deltaTime;
            Debug.Log(shape.radius);
            shape.radius = Mathf.Lerp(startRadius, endRadius, t / duration);
            yield return null;
        }
        shape.radius = endRadius;
    }

    private void HandleGameOver(AGPEvent e)
    {
        DestroyEffect();
        gameHasStarted = false;
        isRotating = false;
    }

    private void RegisterListeners()
    {
        ServicesLocator.EventManager.Register<GameStart>(InitializePowers);
        ServicesLocator.EventManager.Register<GameOver>(HandleGameOver);
        ServicesLocator.EventManager.Register<GameTimeOut>(HandleGameOver);
    }

    private void UnregisterListeners()
    {
        ServicesLocator.EventManager.Unregister<GameStart>(InitializePowers);
        ServicesLocator.EventManager.Unregister<GameOver>(HandleGameOver);
        ServicesLocator.EventManager.Unregister<GameTimeOut>(HandleGameOver);
    }
}

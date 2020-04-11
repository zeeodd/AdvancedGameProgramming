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
        if (collision.tag == "Goal")
        {
            ServicesLocator.EventManager.Fire(new GoalScored(collision.name));
        }
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
    }

    public void ResetMomentum()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}

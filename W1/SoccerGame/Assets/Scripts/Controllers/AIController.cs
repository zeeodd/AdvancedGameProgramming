using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // === PUBLIC ===
    public float speed; // Controls player speed
    public GameObject ball; // Ball to follow

    // === PRIVATE ===
    private Rigidbody2D rb; // 2D rigidbody to grab
    private Vector2 direction;

    void Start()
    {
        if (ball == null) Error.PrintError("Ball Gameobject not hooked up");

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction = (ball.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }

    public void MoveTowardsBall()
    {
        direction = (ball.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // === PRIVATE ===
    private Rigidbody2D rb; // 2D rigidbody to grab
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTowardsBall(GameObject ball, float speed)
    {
        direction = (ball.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}

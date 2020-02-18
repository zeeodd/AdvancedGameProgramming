using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // === PUBLIC ===
    public float speed = 25; // Controls player speed

    // === PRIVATE ===
    private Rigidbody2D rb; // 2D rigidbody to grab
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer()
    {
        if (Input.GetMouseButton(0))
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            direction = (mousePosition - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "AI" && rb.velocity.magnitude > 8.0f)
        {
            ServicesLocator.EventManager.Fire(new PlayerCollision());
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // === PUBLIC ===
    public float speed; // Controls player speed

    // === PRIVATE ===
    private Rigidbody2D rb; // 2D rigidbody to grab
    private Vector2 direction;
    private Vector3 mousePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            direction = (mousePosition - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
        }
    }
}

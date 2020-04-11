using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class AIController : MonoBehaviour
{
    #region Variables

    private Rigidbody2D rb; // 2D rigidbody to grab
    private Vector2 direction;

    private BehaviorTree.Tree<AIController> _aggressiveTree;
    #endregion

    #region Lifecyle Management
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (true) // TODO
        {
            _aggressiveTree = new Tree<AIController>
            (
            new Selector<AIController>
                (
                    new FollowingPlayer()
                )
            );
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Functions

    public void MoveTowardsBall(GameObject ball, float speed)
    {
        direction = (ball.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }

    public void FollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
    }

    #endregion 

}

#region Tree Nodes
public class FollowingPlayer : BehaviorTree.Node<AIController>
{
    public override bool Update(AIController context)
    {
        context.FollowPlayer();

        return true;
    }
}
#endregion

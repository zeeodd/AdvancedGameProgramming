using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class RefereeController : MonoBehaviour
{
    #region Variables
    private Rigidbody2D rb;
    private AudioSource source;
    private Vector2 direction;

    public bool collisionBool = false;
    private bool isPlayerCloser = false;

    private List<SoccerPlayer> soccerPlayers = new List<SoccerPlayer>();
    private BehaviorTree.Tree<RefereeController> _tree;
    #endregion

    #region Lifecycle Management
    private void Awake()
    {
        ServicesLocator.EventManager.Register<PlayerCollision>(HandlePlayerCollision);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();

        foreach (SoccerPlayer ai in ServicesLocator.AIPlayers)
        {
            if (ai.GetAIType() == "Referee")
            {
                continue;
            }

            soccerPlayers.Add(ai);
        }

        foreach (SoccerPlayer player in ServicesLocator.UserPlayer)
        {
            soccerPlayers.Add(player);
        }

        // Player collision Tree
        var playerCollisionTree = new Tree<RefereeController>
        (
            new Sequence<RefereeController>
            (
                new HavePlayersCollided(),
                new BlowWhistle(true)
            )
        );

        // Base Tree
        _tree = new Tree<RefereeController>
        (
            new Selector<RefereeController>
            (
                playerCollisionTree,
                new FollowingBall()
            )
        );
    }

    public void Update()
    {
        _tree.Update(this);
    }

    public void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<PlayerCollision>(HandlePlayerCollision);
    }
    #endregion

    #region Functions
    public void BlowWhistle(bool Bool)
    {
        if (Bool)
        {
            source.Play();
            ServicesLocator.GameManager.isFoulCalled = true;
        }
        collisionBool = false;
    }
    
    public void HandlePlayerCollision(AGPEvent e)
    {
        collisionBool = true;
    }

    public void FollowBall()
    {
        // Get ball objects
        GameObject ball = GameObject.Find("Ball");
        float speed = 2.5f;

        // Get distance to ball
        float ballDist = Vector3.Distance(ball.transform.position, transform.position);

        // Find the closest soccer player
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (SoccerPlayer sp in soccerPlayers)
        {
            float dist = Vector3.Distance(sp.GetPosition(), currentPos);
            if (dist < minDist)
            {
                minDist = dist;
            }
        }

        // Check whether a player or the ball is closer
        if (minDist < ballDist)
        {
            isPlayerCloser = true;
        }
        else
        {
            isPlayerCloser = false;
        }

        // Move accordingly
        if (!isPlayerCloser)
        {
            if (ballDist > 2.5f)
            {
                direction = (ball.transform.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
            
        } 
        else if (isPlayerCloser)
        {
            if (minDist < 5.0f)
            {
                foreach (SoccerPlayer sp in soccerPlayers)
                {
                    direction = (sp.GetPosition() + transform.position).normalized;
                    rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
                }
            } 
            else
            {
                direction = (ball.transform.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
            }
            
        }

    }
    public void Shake()
    {
        var jitter = (0.1f * Random.insideUnitSphere);
        jitter.z = 0;

        transform.position += jitter;
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion
}

#region Tree Nodes
public class HavePlayersCollided : BehaviorTree.Node<RefereeController>
{
    public override bool Update(RefereeController context)
    {
        return context.collisionBool;
    }
}

public class BlowWhistle : BehaviorTree.Node<RefereeController>
{
    private bool collisionBool;

    public BlowWhistle(bool collisionBool)
    {
        this.collisionBool = collisionBool;
    }

    public override bool Update(RefereeController context)
    {
        context.BlowWhistle(collisionBool);
        context.Shake();

        return true;
    }
}

public class FollowingBall : BehaviorTree.Node<RefereeController>
{
    public override bool Update(RefereeController context)
    {
        context.FollowBall();

        return true;
    }
}
#endregion

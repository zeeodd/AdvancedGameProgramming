using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoccerPlayer
{
    #region Variables
    public bool isBlueTeam;
    public bool isAI;

    private const float MOVEMENT_SPEED = 5.0f;
    private GameObject _gameObject;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    public Vector2 _initialPosition;
    #endregion

    #region Lifecycle Management
    protected SoccerPlayer(GameObject gameObject)
    {
        _gameObject = gameObject;
        _rigidbody2D = _gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = _gameObject.GetComponent<SpriteRenderer>();
        _initialPosition = Vector2.zero;
    }

    public abstract void Update();

    public void Destroy()
    {
        UnityEngine.Object.Destroy(_gameObject);
    }
    #endregion

    #region Basic Functionality

    public SoccerPlayer SetAI(bool aiBool)
    {
        isAI = aiBool;
        if (isAI)
        {
            _gameObject.AddComponent<AIController>();
        }
        else
        {
            _gameObject.AddComponent<InputController>();
        }

        return this;
    }

    public SoccerPlayer SetPosition(float x, float y)
    {
        _rigidbody2D.velocity = Vector2.zero;
        _gameObject.transform.position = new Vector2(x, y);
        _initialPosition = _gameObject.transform.position;

        return this;
    }

    public SoccerPlayer SetTag(string s)
    {
        _gameObject.gameObject.tag = s;

        return this;
    }
    #endregion
}

public class AIPlayer : SoccerPlayer
{

    #region Lifecycle Management

    public AIPlayer(GameObject gameObject) : base(gameObject) { }

    public override void Update()
    {
        this.SetAI(true);
    }

    #endregion

}

public class UserPlayer : SoccerPlayer
{

    #region Lifecycle Management

    public UserPlayer(GameObject gameObject) : base(gameObject) { }

    public override void Update()
    {
        this.SetAI(false);
    }

    #endregion

}

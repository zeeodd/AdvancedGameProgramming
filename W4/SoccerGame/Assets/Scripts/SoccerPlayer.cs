using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoccerPlayer
{
    #region Variables
    public bool isBlueTeam;
    public string aiType;

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

    public SoccerPlayer SetAIType(string s)
    {
        aiType = s;
        if (s == "Player")
        {
            _gameObject.AddComponent<InputController>();
        }
        else if (s == "Enemy")
        {
            _gameObject.AddComponent<AIController>();
        }
        else if (s == "Referee")
        {
            _gameObject.AddComponent<RefereeController>();
        }

        return this;
    }

    public string GetAIType()
    {
        string s = "";

        if (_gameObject.GetComponent<InputController>())
        {
            s = "Player";
        }
        else if (_gameObject.GetComponent<AIController>())
        {
            s = "Enemy";
        }
        else if (_gameObject.GetComponent<RefereeController>())
        {
            s = "Referee";
        }

        return s;
    }

    public SoccerPlayer SetPosition(float x, float y)
    {
        _gameObject.transform.position = new Vector2(x, y);
        _initialPosition = _gameObject.transform.position;

        return this;
    }

    public Vector3 GetPosition()
    {
        return _gameObject.transform.position;
    }

    public void ResetMomentum()
    {
        _rigidbody2D.velocity = Vector2.zero;
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
        this.SetAIType("Enemy");
    }

    #endregion

}

public class UserPlayer : SoccerPlayer
{

    #region Lifecycle Management

    public UserPlayer(GameObject gameObject) : base(gameObject) { }

    public override void Update()
    {
        this.SetAIType("Player");
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public GameObject ball;

    [Range(1f, 4f)]
    public int blueTeamNumber;

    [Range(1f, 4f)]
    public int redTeamNumber;

    [Range(1f, 4f)]
    public int aiMovementSpeed;
    #endregion

    public void Awake()
    {
        ServicesLocator.GameManager = this;
        ServicesLocator.AIManager = new AIManager();
        ServicesLocator.InputManager = new InputManager();
        ServicesLocator.UserPlayer = new List<SoccerPlayer>();
        ServicesLocator.AIPlayers = new List<SoccerPlayer>();

        CreatePlayers();
    }

    public void Start()
    {
        ServicesLocator.AIManager.Initialize();
        ServicesLocator.InputManager.Initialize();
    }

    public void Update()
    {
        ServicesLocator.InputManager.MovePlayer();
        ServicesLocator.AIManager.MoveTowardsBall(ball, aiMovementSpeed);
    }

    private void CreatePlayers()
    {
        for (int i = 0; i < blueTeamNumber; i++)
        {
            if (i == 0)
            {
                var gameobj = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
                ServicesLocator.UserPlayer.Add(new UserPlayer(gameobj).SetTag("Player").SetAI(false));
            }
            else
            {
                var gameobj = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
                ServicesLocator.AIPlayers.Add(new AIPlayer(gameobj).SetPosition(-8,i).SetTag("AI").SetAI(true));
            }
        }

        for (int i = 0; i < redTeamNumber; i++)
        {
            var gameobj = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
            ServicesLocator.AIPlayers.Add(new AIPlayer(gameobj).SetPosition(8, i).SetTag("AI").SetAI(true));
        }
    }
}

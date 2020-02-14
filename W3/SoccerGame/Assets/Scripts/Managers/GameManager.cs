using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TextMeshProUGUI score;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI readyText;

    public GameObject titleScreen;

    public GameObject gameScreen;

    public GameObject endScreen;

    public float pauseDuration = 2.0f;
    private bool isGamePaused = false;

    private FiniteStateMachine<GameManager> _GameManagerStateMachine;
    #endregion

    #region Game Cycle
    public void Awake()
    {
        InitializeServices();
    }

    public void Update()
    {
        _GameManagerStateMachine.Update();
    }

    private void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<GameOver>(HandleGameOver);
        ServicesLocator.EventManager.Unregister<GameTimeOut>(HandleGameOver);
        ServicesLocator.EventManager.Unregister<GoalScored>(OnGoalScored);
    }
    #endregion

    #region Functions
    private void InitializeServices()
    {
        ServicesLocator.GameManager = this;
        ServicesLocator.AIManager = new AIManager();
        ServicesLocator.InputManager = new InputManager();
        ServicesLocator.ScoreManager = new ScoreManager();
        ServicesLocator.UserPlayer = new List<SoccerPlayer>();
        ServicesLocator.AIPlayers = new List<SoccerPlayer>();
        ServicesLocator.EventManager = new EventManager();

        ServicesLocator.ScoreManager.Initialize(score, timer, readyText);

        _GameManagerStateMachine = new FiniteStateMachine<GameManager>(this);
        _GameManagerStateMachine.TransitionTo<TitleScreenState>();
    }

    private void CreatePlayers()
    {
        for (int i = 0; i < blueTeamNumber; i++)
        {
            if (i == 0)
            {
                var gameobj = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
                ServicesLocator.UserPlayer.Add(new UserPlayer(gameobj).SetPosition(-7, i).SetTag("Player").SetAI(false));
            }
            else
            {
                var gameobj = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
                ServicesLocator.AIPlayers.Add(new AIPlayer(gameobj).SetPosition(-7,i).SetTag("AI").SetAI(true));
            }
        }

        for (int i = 0; i < redTeamNumber; i++)
        {
            var gameobj = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
            ServicesLocator.AIPlayers.Add(new AIPlayer(gameobj).SetPosition(7, i).SetTag("AI").SetAI(true));
        }

    }

    private void InitializeTitleScreen()
    {
        titleScreen.SetActive(true);
        gameScreen.SetActive(false);
        endScreen.SetActive(false);
    }

    private void InitializeGameScreen()
    {
        CreatePlayers();

        ServicesLocator.InputManager.Initialize();
        ServicesLocator.AIManager.Initialize();

        titleScreen.SetActive(false);
        gameScreen.SetActive(true);
        endScreen.SetActive(false);
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        ServicesLocator.ScoreManager.DisableReadyText(readyText);
    }
    #endregion

    #region Event Handler Functions
    private void HandleGameOver(AGPEvent e)
    {
        _GameManagerStateMachine.TransitionTo<GameOverState>();
    }

    private void OnGoalScored(AGPEvent e)
    {
        Invoke("ResumeGame", pauseDuration);
        ServicesLocator.ScoreManager.EnableReadyText(readyText);

        isGamePaused = true;

        ball.GetComponent<Ball>().ResetMomentum();
        ball.GetComponent<Ball>().ResetPosition();

        foreach (SoccerPlayer ai in ServicesLocator.AIPlayers)
        {
            ai.SetPosition(ai._initialPosition.x, ai._initialPosition.y);
        }

        foreach (SoccerPlayer player in ServicesLocator.UserPlayer)
        {
            player.SetPosition(player._initialPosition.x, player._initialPosition.y);
        }

    }
    #endregion

    #region States
    // Parent State
    private class GameState : FiniteStateMachine<GameManager>.State
    {
        public override void OnEnter() 
        {
            ServicesLocator.EventManager.Register<GameOver>(Context.HandleGameOver);
            ServicesLocator.EventManager.Register<GameTimeOut>(Context.HandleGameOver);
            ServicesLocator.EventManager.Register<GoalScored>(Context.OnGoalScored);
        }
        public override void Update() { }
        public override void OnExit() { }
    }

    private class TitleScreenState : GameState
    {
        public override void OnEnter()
        {
            Context.InitializeTitleScreen();
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TransitionTo<InGameState>();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    private class InGameState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context.InitializeGameScreen();
            Context.ball.GetComponent<Ball>().ResetPosition();
            ServicesLocator.ScoreManager.Initialize(Context.score, Context.timer, Context.readyText);
        }

        public override void Update()
        {
            if (!Context.isGamePaused)
            {
                ServicesLocator.AIManager.MoveTowardsBall(Context.ball, Context.aiMovementSpeed);
                ServicesLocator.InputManager.MovePlayer();
            }
            ServicesLocator.ScoreManager.UpdateScore(Context.score);
            ServicesLocator.ScoreManager.UpdateTimer(Context.timer);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    private class GameOverState : GameState
    {
        public override void OnEnter()
        {
            Context.gameScreen.SetActive(false);
            Context.endScreen.SetActive(true);

            ServicesLocator.AIManager.Destroy();
            ServicesLocator.InputManager.Destroy();
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TransitionTo<InGameState>();
            }
        }

        public override void OnExit()
        {
            ServicesLocator.UserPlayer.Clear();
            ServicesLocator.AIPlayers.Clear();
            ServicesLocator.ScoreManager.Destroy();
        }
    }
    #endregion
}

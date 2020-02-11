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

    public GameObject titleScreen;

    public GameObject gameScreen;

    public GameObject endScreen;

    // Example state check
    public bool IsInGame()
    {
        return _GameManagerStateMachine.CurrentState.GetType() == typeof(GameStart);
    }

    private FiniteStateMachine<GameManager> _GameManagerStateMachine;

    private bool atTitleScreen = true;
    private bool gameScreenObjectsReady = false;
    private bool gameOver = false;
    private bool hasWaitedOneFrame = false;
    #endregion

    private class GameState : FiniteStateMachine<GameManager>.State
    {
        public override void OnEnter()
        {
            // initialization
            // Probably want to put the start code here
        }
        public override void Update()
        {
            // update
        }
        public override void OnExit()
        {
            // on exit
        }
    }

    private class TitleScreen : GameState
    {
        public override void OnEnter()
        {
            //base.OnEnter();

            Context.InitializeTitleScreen();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    #region Game Cycle
    public void Awake()
    {
        ServicesLocator.GameManager = this;
        ServicesLocator.AIManager = new AIManager();
        ServicesLocator.InputManager = new InputManager();
        ServicesLocator.ScoreManager = new ScoreManager();
        ServicesLocator.UserPlayer = new List<SoccerPlayer>();
        ServicesLocator.AIPlayers = new List<SoccerPlayer>();
        ServicesLocator.EventManager = new EventManager();

        _GameManagerStateMachine = new FiniteStateMachine<GameManager>(this);
        _GameManagerStateMachine.TransitionTo<TitleScreen>();
        // Use Context to use functions from THIS GameManager (via Context)

        InitializeTitleScreen();
    }

    public void Start()
    {
        ServicesLocator.ScoreManager.Initialize(score);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && atTitleScreen)
        {
            CreatePlayers();
            ServicesLocator.InputManager.Initialize();
            atTitleScreen = false;
        }
        
        if (!atTitleScreen && !gameScreenObjectsReady)
        {
            ServicesLocator.EventManager.Fire(new ExitTitleScreen());
        }

        if (gameScreenObjectsReady && !gameOver)
        {
            if (!hasWaitedOneFrame)
            {
                StartCoroutine(GreenFlagMovement());
            } 
            else
            {
                ServicesLocator.AIManager.MoveTowardsBall(ball, aiMovementSpeed);
                ServicesLocator.InputManager.MovePlayer();
            }
            ServicesLocator.ScoreManager.UpdateScore(score);
            ServicesLocator.ScoreManager.CheckGameOver();
        }
    }
    #endregion

    #region Functions
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

    private void InitializeTitleScreen()
    {
        ServicesLocator.EventManager.Register<ExitTitleScreen>(InitializeGameScreen);
        ServicesLocator.EventManager.Register<GameOver>(HandleGameOver);
        titleScreen.SetActive(true);
        gameScreen.SetActive(false);
        endScreen.SetActive(false);
    }

    private void InitializeGameScreen(AGPEvent e)
    {
        ServicesLocator.EventManager.Unregister<ExitTitleScreen>(InitializeGameScreen);

        ServicesLocator.AIManager.Initialize();

        titleScreen.SetActive(false);
        gameScreen.SetActive(true);

        gameScreenObjectsReady = true;
    }

    private void HandleGameOver(AGPEvent e)
    {
        gameScreen.SetActive(false);
        endScreen.SetActive(true);

        foreach (AIPlayer aiplayer in ServicesLocator.AIPlayers)
        {
            aiplayer.Destroy();
        }

        foreach (UserPlayer userplayer in ServicesLocator.UserPlayer)
        {
            userplayer.Destroy();
        }

        ServicesLocator.EventManager.Unregister<GameOver>(HandleGameOver);

        gameOver = true;
    }

    IEnumerator GreenFlagMovement()
    {
        yield return 0;

        hasWaitedOneFrame = true;
    }
    #endregion
}

﻿using System.Collections;
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
    private AudioSource titleScreenAudioSource;

    public GameObject gameScreen;
    private AudioSource gameScreenAudioSource;

    public GameObject endScreen;
    private AudioSource endScreenAudioSource;

    public float pauseTimer = 0f;
    public float pauseDuration = 2.0f;
    public bool isGamePaused = false;
    public bool isFoulCalled = false;

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
                GameObject gameobj = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
                ServicesLocator.UserPlayer.Add(new UserPlayer(gameobj).SetPosition(-7, i).SetTag("Player").SetAIType("Player"));
            }
            else
            {
                GameObject gameobj = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
                ServicesLocator.AIPlayers.Add(new AIPlayer(gameobj).SetPosition(-7,i).SetTag("AI").SetAIType("Enemy"));
            }
        }

        for (int i = 0; i < redTeamNumber; i++)
        {
            GameObject gameobj = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
            ServicesLocator.AIPlayers.Add(new AIPlayer(gameobj).SetPosition(7, i).SetTag("AI").SetAIType("Enemy"));
        }

        GameObject referee = Instantiate(Resources.Load<GameObject>("Prefabs/Referee"));
        ServicesLocator.AIPlayers.Add(new AIPlayer(referee).SetPosition(0, 3).SetTag("AI").SetAIType("Referee"));

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
        isFoulCalled = false;

        ball.GetComponent<Ball>().ResetPosition();

        foreach (SoccerPlayer ai in ServicesLocator.AIPlayers)
        {
            ai.SetPosition(ai._initialPosition.x, ai._initialPosition.y);
        }

        foreach (SoccerPlayer player in ServicesLocator.UserPlayer)
        {
            player.SetPosition(player._initialPosition.x, player._initialPosition.y);
        }
        
        ServicesLocator.ScoreManager.DisableReadyText(readyText);
    }

    private void HandlePausedGame()
    {
        if (isGamePaused || isFoulCalled)
        {
            pauseTimer += Time.deltaTime;

            ball.GetComponent<Ball>().ResetMomentum();

            foreach (SoccerPlayer ai in ServicesLocator.AIPlayers)
            {
                ai.ResetMomentum();
            }

            foreach (SoccerPlayer player in ServicesLocator.UserPlayer)
            {
                player.ResetMomentum();
            }

            if (pauseTimer >= pauseDuration)
            {
                pauseTimer = 0f;
                ResumeGame();
            }
        }
    }
    #endregion

    #region Event Handler Functions
    private void HandleGameOver(AGPEvent e)
    {
        _GameManagerStateMachine.TransitionTo<GameOverState>();
    }

    private void OnGoalScored(AGPEvent e)
    {
        ServicesLocator.ScoreManager.EnableReadyText(readyText);

        isGamePaused = true;

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
            Context.titleScreenAudioSource = Context.titleScreen.GetComponent<AudioSource>();
            Context.titleScreenAudioSource.Play();
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

            Context.titleScreenAudioSource.Stop();
        }
    }

    private class InGameState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            ServicesLocator.EventManager.Fire(new GameStart());

            Context.InitializeGameScreen();
            Context.isFoulCalled = false;
            Context.isGamePaused = false;
            Context.ball.GetComponent<Ball>().ResetPosition();
            ServicesLocator.ScoreManager.Initialize(Context.score, Context.timer, Context.readyText);
            Context.gameScreenAudioSource = Context.gameScreen.GetComponent<AudioSource>();
            Context.gameScreenAudioSource.Play();
        }

        public override void Update()
        {

            if (!Context.isGamePaused && !Context.isFoulCalled)
            {
                ServicesLocator.AIManager.MoveTowardsBall(Context.ball, Context.aiMovementSpeed);
                ServicesLocator.InputManager.MovePlayer();
            } 
            else
            {
                Context.HandlePausedGame();
            }
            ServicesLocator.ScoreManager.UpdateScore(Context.score);
            ServicesLocator.ScoreManager.UpdateTimer(Context.timer);
        }

        public override void OnExit()
        {
            base.OnExit();

            Context.gameScreenAudioSource.Stop();
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

            Context.endScreenAudioSource = Context.endScreen.GetComponent<AudioSource>();
            Context.endScreenAudioSource.Play();
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
            Context.endScreenAudioSource.Stop();
        }
    }
    #endregion
}

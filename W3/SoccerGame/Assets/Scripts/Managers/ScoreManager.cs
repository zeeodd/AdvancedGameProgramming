using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager
{
    #region Variables
    public int redScore;
    public int blueScore;
    public const int maxScore = 3;

    public float timer = 0;
    public const float maxTime = 10f;
    #endregion

    #region Lifecycle Management
    public void  Initialize(TextMeshProUGUI score)
    {
        redScore = 0;
        blueScore = 0;
        score.text = "\tblue: " + blueScore + "\t\t\t\t\t\t\t\t\tred: " + redScore;
        ServicesLocator.EventManager.Register<GameStart>(OnGameStart);
        ServicesLocator.EventManager.Register<GoalScoredOnBlueTeam>(OnGoalScoredOnBlueTeam);
        ServicesLocator.EventManager.Register<GoalScoredOnRedTeam>(OnGoalScoredOnRedTeam);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= maxTime)
        {
            ServicesLocator.EventManager.Fire(new GameTimeOut(blueScore, redScore));
            // TO GET SCORE: var redScore = ((GameTimedOut) e).redScore;
        }
    }

    public void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<GoalScoredOnBlueTeam>(OnGoalScoredOnBlueTeam);
        ServicesLocator.EventManager.Unregister<GoalScoredOnRedTeam>(OnGoalScoredOnRedTeam);
    }
    #endregion

    #region Functionality
    public void UpdateScore(TextMeshProUGUI score)
    {
        score.text = "\tblue: " + blueScore + "\t\t\t\t\t\t\t\t\tred: " + redScore;
    }

    public void OnGoalScoredOnBlueTeam(AGPEvent e)
    {
        redScore++;
    }

    public void OnGoalScoredOnRedTeam(AGPEvent e)
    {
        blueScore++;
    }

    public void CheckGameOver()
    {
        if (blueScore == 3 || redScore == 3)
        {
            ServicesLocator.EventManager.Fire(new GameOver());
        }
    }

    public void OnGameStart(AGPEvent e)
    {
        timer = 0;
    }
    #endregion
}

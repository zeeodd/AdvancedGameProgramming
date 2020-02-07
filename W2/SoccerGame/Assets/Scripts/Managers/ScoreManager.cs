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
    #endregion

    #region Lifecycle Management
    public void  Initialize(TextMeshProUGUI score)
    {
        redScore = 0;
        blueScore = 0;
        score.text = "\tblue: " + blueScore + "\t\t\t\t\t\t\t\t\tred: " + redScore;
        ServicesLocator.EventManager.Register<GoalScoredOnBlueTeam>(OnGoalScoredOnBlueTeam);
        ServicesLocator.EventManager.Register<GoalScoredOnRedTeam>(OnGoalScoredOnRedTeam);
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
    #endregion
}

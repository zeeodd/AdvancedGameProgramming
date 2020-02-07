using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager
{
    #region Variables
    public int redScore;
    public int blueScore;
    #endregion

    #region Lifecycle Management
    public void  Initialize(TextMeshProUGUI score)
    {
        redScore = 0;
        blueScore = 0;
        score.text = "blue:\t"+blueScore+"\nred:\t"+redScore;
        ServicesLocator.EventManager.Register<GoalScoredOnBlueTeam>(OnGoalScoredOnBlueTeam);
        ServicesLocator.EventManager.Register<GoalScoredOnRedTeam>(OnGoalScoredOnRedTeam);
    }

    public void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<GoalScoredOnBlueTeam>(OnGoalScoredOnBlueTeam);
        ServicesLocator.EventManager.Register<GoalScoredOnRedTeam>(OnGoalScoredOnRedTeam);
    }
    #endregion

    #region Functionality
    public void UpdateScore(TextMeshProUGUI score)
    {
        score.text = "blue:\t" + blueScore + "\nred:\t" + redScore;
    }

    public void OnGoalScoredOnBlueTeam(AGPEvent e)
    {
        redScore++;
    }

    public void OnGoalScoredOnRedTeam(AGPEvent e)
    {
        blueScore++;
    }
    #endregion
}

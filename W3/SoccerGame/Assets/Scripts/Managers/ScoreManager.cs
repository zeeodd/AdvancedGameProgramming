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
    public const float startTime = 25f;
    #endregion

    #region Lifecycle Management
    public void Initialize(TextMeshProUGUI scoreobj, TextMeshProUGUI timerobj)
    {
        redScore = 0;
        blueScore = 0;
        timer = startTime;
        scoreobj.text = "\tblue: " + blueScore + "\t\t\t\t\t\t\t\t\tred: " + redScore;
        timerobj.text = startTime.ToString();
        ServicesLocator.EventManager.Register<GoalScoredOnBlueTeam>(OnGoalScoredOnBlueTeam);
        ServicesLocator.EventManager.Register<GoalScoredOnRedTeam>(OnGoalScoredOnRedTeam);
    }

    public void Destroy()
    {
        ServicesLocator.EventManager.Unregister<GoalScoredOnBlueTeam>(OnGoalScoredOnBlueTeam);
        ServicesLocator.EventManager.Unregister<GoalScoredOnRedTeam>(OnGoalScoredOnRedTeam);
    }
    #endregion

    #region Functions
    public void UpdateScore(TextMeshProUGUI scoreobj)
    {
        scoreobj.text = "\tblue: " + blueScore + "\t\t\t\t\t\t\t\t\tred: " + redScore;

        if (blueScore == 3 || redScore == 3)
        {
            ServicesLocator.EventManager.Fire(new GameOver());
        }
    }

    public void UpdateTimer(TextMeshProUGUI timerobj)
    {
        timer -= Time.deltaTime;

        timerobj.text = StyleTimer(timer);

        if (timer <= 0f)
        {
            ServicesLocator.EventManager.Fire(new GameTimeOut(blueScore, redScore));
            // TO GET SCORE: var redScore = ((GameTimedOut) e).redScore;
        }
    }

    private string StyleTimer(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        return niceTime;
    }
    #endregion

    #region Event Handler Functions
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

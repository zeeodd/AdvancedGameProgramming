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
    public void Initialize(TextMeshProUGUI scoreobj, TextMeshProUGUI timerobj, TextMeshProUGUI readytextobj)
    {
        redScore = 0;
        blueScore = 0;
        timer = startTime;
        scoreobj.text = "\tblue: " + blueScore + "\t\t\t\t\t\t\t\t\tred: " + redScore;
        timerobj.text = startTime.ToString();
        readytextobj.gameObject.SetActive(false);
        ServicesLocator.EventManager.Register<GoalScored>(OnGoalScored);
    }

    public void Destroy()
    {
        ServicesLocator.EventManager.Unregister<GoalScored>(OnGoalScored);
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

    public void EnableReadyText(TextMeshProUGUI readytextobj)
    {
        readytextobj.gameObject.SetActive(true);
    }

    public void DisableReadyText(TextMeshProUGUI readytextobj)
    {
        readytextobj.gameObject.SetActive(false);
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
    public void OnGoalScored(AGPEvent e)
    {
        var goalName = ((GoalScored)e).goalName;

        if (goalName == "PlayerGoal")
        {
            redScore++;
        }
        else if (goalName == "EnemyGoal")
        {
            blueScore++;
        }
    }
    #endregion
}

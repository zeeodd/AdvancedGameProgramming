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
    }
    public void Start()
    {
        ServicesLocator.EventManager.Register<GoalScored>(IncrementScore);
    }
    public void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<GoalScored>(IncrementScore);
    }
    #endregion

    #region Functionality
    public void UpdateScore(TextMeshProUGUI score)
    {
        score.text = "blue:\t" + blueScore + "\nred:\t" + redScore;
    }

    public void IncrementScore(AGPEvent e)
    {
        redScore += 1;
        Error.PrintError("Score Has Increased!");
    }
    #endregion
}

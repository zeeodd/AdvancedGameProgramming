using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager
{
    public List<GameObject> listOfAI = new List<GameObject>();

    public void Initialize()
    {
        foreach (GameObject ai in GameObject.FindGameObjectsWithTag("AI"))
        {
            listOfAI.Add(ai);
        }
    }

    public void MoveTowardsBall(GameObject ball, float speed)
    {
        if (listOfAI.Count == 0) ZLog.Print("There are no enemies left!");
        else
        {
            foreach (GameObject ai in ServicesLocator.AIManager.listOfAI)
            {
                ai.GetComponent<AIController>().MoveTowardsBall(ball, speed);
            }
        }
    }

    public void Destroy()
    {
        foreach (GameObject aiplayer in listOfAI)
        {
            aiplayer.GetComponent<AIController>().Destroy();
        }

        listOfAI.Clear();
    }

}

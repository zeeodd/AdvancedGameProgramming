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
                if (ai.GetComponent<RefereeController>())
                {
                    continue;
                } 
                else if (ai.GetComponent<AIController>())
                {
                    ai.GetComponent<AIController>().MoveTowardsBall(ball, speed);
                }
            }
        }
    }

    public void Destroy()
    {
        foreach (GameObject aiplayer in listOfAI)
        {
            if (aiplayer.GetComponent<RefereeController>())
            {
                aiplayer.GetComponent<RefereeController>().Destroy();
            }
            else if (aiplayer.GetComponent<AIController>())
            {
                aiplayer.GetComponent<AIController>().Destroy();
            }
        }

        listOfAI.Clear();
    }

}

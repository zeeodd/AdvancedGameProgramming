using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public GameObject player;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void MovePlayer()
    {
        player.GetComponent<InputController>().MovePlayer();
    }
}

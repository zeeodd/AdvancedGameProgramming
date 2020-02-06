using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServicesLocator
{
    public static GameManager GameManager;
    public static AIManager AIManager;
    public static InputManager InputManager;
    public static ScoreManager ScoreManager;
    public static List<SoccerPlayer> AIPlayers;
    public static List<SoccerPlayer> UserPlayer;
    public static EventManager EventManager;
}

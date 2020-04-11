using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ZLog
{
    public static void Print(object obj)
    {
        Debug.Log(obj);
    }

    public static void Error(object obj)
    {
        Debug.LogError(obj);
    }

    public static void Warning(object obj)
    {
        Debug.LogWarning(obj);
    }
}

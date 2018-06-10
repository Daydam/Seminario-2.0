using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class EndRoundHandler
{
    public static float timeScale = .3f;

    public static void ResetTime()
    {
        Time.timeScale = 1;
    }

    public static void ApplyTimeChanges()
    {
        Time.timeScale = timeScale;
    }
}

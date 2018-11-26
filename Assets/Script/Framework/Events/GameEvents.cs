using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Events
{
    public static class GameEvents
    {
        public const string GameStart = "PlayersLoaded";
        public const string RoundReset = "RoundReset";
    }

    public static class UIEvents
    {
        public const string StartRound = "UI_StartRound";
    }
}
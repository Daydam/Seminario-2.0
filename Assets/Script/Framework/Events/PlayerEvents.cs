using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Events
{
    public static class PlayerEvents
    {
        /// <summary>
        /// 0 - PlayerDying
        /// 1 - DeathType
        /// 2 - WasPushed?
        /// 3 - PlayerKiller (tag del jugador)
        /// </summary>
        public const string Death = "PlayerDeath";

        /// <summary>
        /// 0 - PlayerDamaged
        /// 1 - PlayerAttacker (tag del jugador)
        /// 2 - Amount
        /// </summary>
        public const string TakeDamage = "PlayerTakeDamage";


    }
}

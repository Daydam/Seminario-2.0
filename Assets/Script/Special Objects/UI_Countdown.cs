using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;

/// <summary>
/// Un cromosoma, dos cromosomas, tres cromosomas...
/// </summary>
public class UI_Countdown : MonoBehaviour
{
	public void AStartRound()
    {
        EventManager.Instance.DispatchEvent(UIEvents.StartRound);
    }
}

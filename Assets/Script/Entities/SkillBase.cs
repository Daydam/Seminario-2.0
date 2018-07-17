using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SkillBase : MonoBehaviour
{
    protected SkillStateIndicator _feedback;

    /// <summary>
    /// * Verde > usable
    /// * Verde intermitente > llenando cargas pero usable
    /// * Amarillo > en uso
    /// * Rojo > no se puede usar
    /// * Apagado > Dron disableado
    /// </summary>
    /// <returns> </returns>
    public abstract SkillState GetActualState();
}

public enum SkillState { Unavailable, Available, Active, Reloading, UserDisabled, ERROR }


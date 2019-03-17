using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class StageBase : MonoBehaviour
{
    public abstract void ResetRound();
    public virtual void DestroyStatic() { }
}

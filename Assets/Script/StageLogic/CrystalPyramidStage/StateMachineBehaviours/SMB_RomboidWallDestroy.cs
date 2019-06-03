using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SMB_RomboidWallDestroy : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        EventManager.Instance.DispatchEvent(Firepower.Events.CrystalPyramidEvents.DestructibleWallDestroyEnd, animator);
    }
}

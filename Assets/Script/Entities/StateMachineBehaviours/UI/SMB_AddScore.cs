using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SMB_AddScore : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("In");
    }
}

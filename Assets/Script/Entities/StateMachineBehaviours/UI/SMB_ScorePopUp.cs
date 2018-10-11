using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SMB_ScorePopUp : StateMachineBehaviour
{
    public float duration = 2f;
    float _actualTime = 0f;
    bool _visible;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _visible = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (_visible)
        {
            if (_actualTime <= duration)
            {
                _actualTime += Time.deltaTime;
            }
            else
            {
                _visible = false;
                animator.SetTrigger("Out");
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("In");
        animator.ResetTrigger("Out");
        _actualTime = 0;
    }
}

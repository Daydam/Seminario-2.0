using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PhoenixDevelopment;

public class SMB_StatueDestroy : StateMachineBehaviour
{
    DestructibleStatue _statue;
    AnimatorClipInfo _currentClip;
    int _frame;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _statue = animator.GetComponentInParent<DestructibleStatue>();
        _currentClip = animator.GetCurrentAnimatorClipInfo(0).First();
        _frame = _statue.normalSpeedFrame;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (_currentClip.GetCurrentAnimatorClipFrame() == _frame)
        {
            _statue.SetDestrucibleAnimSpeed(true);
        }
    }
}

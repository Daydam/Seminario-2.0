using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FourPartMinimap : MonoBehaviour
{
    Animator _an;

    string _fallAnimationSufix = "_Fall", _dangerAnimationSufix = "_Danger";

    void Awake()
    {
        _an = GetComponent<Animator>();
    }

    /// <summary>
    /// Recieves 0 to 3
    /// </summary>
    /// <param name="indx"> 0 to 3 </param>
    public void SetDangerState(int indx)
    {
        _an.CrossFadeInFixedTime((indx + 1).ToString() + _dangerAnimationSufix, .25f);
    }

    /// <summary>
    /// Recieves 1 to 4
    /// </summary>
    /// <param name="indx"> 0 to 3 </param>
    public void SetFallState(int indx)
    {
        _an.CrossFadeInFixedTime((indx).ToString() + _fallAnimationSufix, .25f);

    }

    public void ResetRound()
    {
        _an.Play("Start");
    }
}

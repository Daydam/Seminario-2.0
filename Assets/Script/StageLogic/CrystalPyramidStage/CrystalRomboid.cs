using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrystalRomboid : MonoBehaviour
{
    Animator _an;
    CrystalPyramidDangerParticle[] _dangerParticles;

    void Start()
    {
        _an = GetComponent<Animator>();
        _dangerParticles = GetComponentsInChildren<CrystalPyramidDangerParticle>();
    }

    public void SetDanger()
    {
        //_an.SetTrigger("Danger");
    }

    public void RomboidFall()
    {
        _an.SetTrigger("Fall");
    }

    public void OnResetRound()
    {
        _an.ResetTrigger("Fall");
        _an.ResetTrigger("Danger");
        _an.Play("Idle");
    }
}

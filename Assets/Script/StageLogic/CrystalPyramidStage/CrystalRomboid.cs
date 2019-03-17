using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrystalRomboid : MonoBehaviour
{
    Animator _an;

    void Start()
    {
        _an = GetComponent<Animator>();
    }

    public void SetDanger()
    {
        _an.SetTrigger("Danger");
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

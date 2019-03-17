using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LaserPyramid : MonoBehaviour
{
    Animator _an;
    readonly string[] _stateNames = new string[] { "Border", "Middle", "Inner", "End" };
    int _actualIndex = 0;

    //test mugriento
    Renderer _rn;

    void Start()
    {
        _an = GetComponent<Animator>();
        _rn = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        //test mugriento
        if (!_rn.enabled) _rn.enabled = true;
    }

    public void OnFall(int indx)
    {
        if (_actualIndex <= indx && indx < _stateNames.Length)
        {
            _actualIndex = indx;
            _an.CrossFadeInFixedTime(_stateNames[_actualIndex], .1f);
        }
    }

    public void OnResetRound()
    {
        _an.Play("Idle");
        _actualIndex = 0;
    }
}

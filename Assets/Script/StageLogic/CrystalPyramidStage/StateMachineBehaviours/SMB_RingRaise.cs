using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SMB_RingRaise : StateMachineBehaviour
{
    FList<Tuple<Player, Rigidbody>> _playerBodies;
    Transform _floor;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _playerBodies = FList.Create<Tuple<Player, Rigidbody>>();
        foreach (var item in GameManager.Instance.Players)
        {
            var rb = item.GetComponent<Rigidbody>();
            _playerBodies += Tuple.Create(item, rb);
            rb.velocity = Vector3.zero;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        foreach (var tuple in _playerBodies)
        {
            //tuple.Item2.velocity = Vector3.zero;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        foreach (var tuple in _playerBodies)
        {
            tuple.Item2.velocity = Vector3.zero;
            tuple.Item2.AddForce(Physics.gravity * 5, ForceMode.VelocityChange);
        }
    }
}

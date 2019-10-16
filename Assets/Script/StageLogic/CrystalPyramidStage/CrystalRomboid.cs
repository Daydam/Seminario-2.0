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

    public Collider[] GetStructuresColliders()
    {
        var destructibles = GetComponentsInChildren<DestructibleBase>(true).SelectMany(x => x.GetComponentsInChildren<Collider>()).ToArray();
        var structures = GetComponentsInChildren<RingStructure>(true).SelectMany(x => x.GetComponentsInChildren<Collider>()).ToArray();
        return destructibles.Concat(structures).ToArray();
    }
    public Collider[] GetStructuresColliders(ObstacleHeight height)
    {
        var destructibles = GetComponentsInChildren<DestructibleBase>(true).Where(x => x.obstacleHeight == height).SelectMany(x => x.GetComponentsInChildren<Collider>()).ToArray();
        var structures = GetComponentsInChildren<RingStructure>(true).Where(x => x.obstacleHeight == height).SelectMany(x => x.GetComponentsInChildren<Collider>()).ToArray();
        return destructibles.Concat(structures).ToArray();
    }
}

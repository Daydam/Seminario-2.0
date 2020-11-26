using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrystalRomboid : MonoBehaviour
{
    Animator _an;
    public Renderer renderer;
    public CrystalPyramidDangerParticle dangerParticle;
    public Transform[] laserPoints;

    void Start()
    {
        _an = GetComponent<Animator>();
        renderer.material.SetFloat("_ActiveDangerZone", 0);

        dangerParticle = FindObjectOfType<CrystalPyramidDangerParticle>();

        if (laserPoints == null)
        {
            Debug.LogError("NO HAY LASER POINTS CARGADOS");
        }


    }

    public void SetDanger(float dangerTime)
    {
        //floor glow
       renderer.material.SetFloat("_ActiveDangerZone", 1);

        //start particle by sending laserPoints
        dangerParticle.Initialize(laserPoints);
        dangerParticle.StartDanger(dangerTime);

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
        dangerParticle.ResetRound();
        renderer.material.SetFloat("_ActiveDangerZone", 0);
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

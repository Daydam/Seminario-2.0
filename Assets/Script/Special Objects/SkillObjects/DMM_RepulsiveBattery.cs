using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DMM_RepulsiveBattery : MonoBehaviour
{
    Player _owner;
    public float repulsiveForce;
    public float radius;
    int _layerMask;

    bool _showGizmos = true;

    public DMM_RepulsiveBattery Spawn(Vector3 spawnPos, Player owner)
    {
        transform.position = spawnPos;
        transform.parent = null;
        var otherPlayerLayers = new int[] { LayerMask.NameToLayer("Player1"), LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Player3"), LayerMask.NameToLayer("Player4") }
                                      .Where(x => x != owner.gameObject.layer)
                                      .ToArray();

        _layerMask = otherPlayerLayers.Aggregate((acum, curr) => acum | (1 << curr));
        _owner = owner;

        ActivateRepulsion();

        return this;
    }

    public static void Initialize(DMM_RepulsiveBattery obj)
    {
        obj.gameObject.SetActive(true);
    }

    public static void Dispose(DMM_RepulsiveBattery obj)
    {
        obj.gameObject.SetActive(false);
    }

    void ReturnToPool()
    {
        RepulsiveBatterySpawner.Instance.ReturnToPool(this);
    }

    public void ActivateRepulsion()
    {
        //do particles

        var particleID = SimpleParticleSpawner.ParticleID.REPULSIVEBATTERY;
        var particle = SimpleParticleSpawner.Instance.particles[particleID].GetComponentInChildren<ParticleSystem>();

        SimpleParticleSpawner.Instance.SpawnParticle(particle.gameObject, transform.position, transform.forward, null);

        var cols = Physics.OverlapSphere(transform.position, radius);

        if (!cols.Any()) return;

        var playersAffected = cols.Select(x => x.GetComponent<Player>()).Where(x => x != null).Where(x => x != _owner).ToArray();

        if (!playersAffected.Any()) return;

        for (int i = 0; i < playersAffected.Length; i++)
        {
            playersAffected[i].ApplyExplosionForce(repulsiveForce, transform.position, radius, _owner);
        }

        Invoke("Destruction", .1f);
    }

    void Destruction()
    {
        ReturnToPool();
    }
}

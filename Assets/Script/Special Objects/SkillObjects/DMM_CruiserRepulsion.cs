using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DMM_CruiserRepulsion : MonoBehaviour
{
    Player _owner;
    public float repulsiveForce = 30;
    public float radius = 5;

    public DMM_CruiserRepulsion Spawn(Vector3 spawnPos, Player owner)
    {
        transform.position = spawnPos;
        transform.parent = null;

        _owner = owner;

        ActivateRepulsion();

        return this;
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
        gameObject.SetActive(false);
    }
}

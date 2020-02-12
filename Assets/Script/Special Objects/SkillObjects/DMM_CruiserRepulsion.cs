using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DMM_CruiserRepulsion : MonoBehaviour
{
    public SO_CruiserFlight skillData;

    Player _owner;

    public DMM_CruiserRepulsion Spawn(Vector3 spawnPos, Player owner, SO_CruiserFlight data)
    {
        skillData = data;

        transform.position = spawnPos;
        transform.parent = null;

        _owner = owner;

        ActivateRepulsion();

        return this;
    }

    public void ActivateRepulsion()
    {
        //do particles

        var particleID = SimpleParticleSpawner.ParticleID.REPULSIVE_BATTERY;
        var particle = SimpleParticleSpawner.Instance.particles[particleID].GetComponentInChildren<ParticleSystem>();

        SimpleParticleSpawner.Instance.SpawnParticle(particle.gameObject, transform.position, transform.forward, null);

        var cols = Physics.OverlapSphere(transform.position, skillData.radius);

        if (!cols.Any()) return;

        var playersAffected = cols.Select(x => x.GetComponent<Player>()).Where(x => x != null).Where(x => x != _owner).ToArray();

        if (!playersAffected.Any()) return;

        for (int i = 0; i < playersAffected.Length; i++)
        {
            playersAffected[i].ApplyExplosionForce(skillData.repulsiveForce, transform.position, skillData.radius, _owner);
        }

        Invoke("Destruction", .1f);
    }

    void Destruction()
    {
        gameObject.SetActive(false);
    }
}

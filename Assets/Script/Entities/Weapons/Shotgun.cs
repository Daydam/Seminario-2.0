﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shotgun : TapWeapon
{
    public float maxDispersionRadius = 0.1f;
    public int pellets = 12;

    public override void Shoot()
    {
        CalculatePelletsDispersion(falloffEnd);
    }

    void CalculatePelletsDispersion(float depth)
    {
        for (int i = 0; i < pellets; i++)
        {
            var dispersionPoint = Random.Range(-maxDispersionRadius, maxDispersionRadius);
            var dir = Owner.transform.forward + (Owner.transform.right * dispersionPoint);

            var b = new HitscanBullet(Owner.transform.position, dir.normalized, Owner, damageFalloff, knockbackFalloff, pellets);
            var part = particle.GetComponentInChildren<ParticleSystem>();
            var speed = part.main.startSpeed.constant * part.main.simulationSpeed;
            var lifeTime = b.objDist / speed;

            b.SpawnParticle(particle, Owner.transform.position, dir, lifeTime);
        }

    }

}

#region Old
/*void CalculatePelletsDispersion(float depth)
{
    for (int i = 0; i < pellets; i++)
    {
        var dispersionPoint = Random.Range(0f, maxDispersionRadius);

        var deltaAngle = Mathf.Atan2(dispersionPoint, depth);

        deltaAngle = i + 1 <= pellets / 2 ? deltaAngle *= -1 : deltaAngle;

        var vectorRotated = Quaternion.Euler(0, deltaAngle, 0) * Owner.transform.forward;

        var b = new HitscanBullet(Owner.transform.position, vectorRotated.normalized, Owner, damageFalloff, knockbackFalloff, pellets);
        var part = particle.GetComponentInChildren<ParticleSystem>();
        var speed = part.main.startSpeed.constant * part.main.simulationSpeed;
        var lifeTime = b.objDist / speed;

        b.SpawnParticle(particle, Owner.transform.position, Owner.transform.rotation * Quaternion.Euler(vectorRotated), lifeTime);
    }

}*/
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
        else if (control.MainWeapon() && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            Shoot();
            currentCooldown = realCooldown;
        }
    }

    public override void Shoot()
    {
        var b = new HitscanBullet(Owner.transform.position, Owner.transform.forward, Owner, damageFalloff, knockbackFalloff);
        var part = particle.GetComponentInChildren<ParticleSystem>();
        var speed = part.main.startSpeed.constant * part.main.simulationSpeed;
        var lifeTime = b.objDist / speed;
        b.SpawnParticle(particle, Owner.transform.position, Owner.transform.rotation, lifeTime);
    }
}

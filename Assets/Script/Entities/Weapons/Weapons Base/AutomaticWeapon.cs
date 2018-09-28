using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
        else if (control.MainWeapon() && !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting)
        {
            Shoot();
            currentCooldown = realCooldown;
        }
    }

    public override void Shoot()
    {
        Owner.ApplyVibration(0, VibrationIntensity, VibrationDuration);
        Owner.ApplyShake(ShakeDuration, ShakeIntensity);

        var b = new HitscanBullet(Owner.transform.position, Owner.transform.forward, Owner, damageFalloff, knockbackFalloff, 1);

        var particleID = SimpleParticleSpawner.ParticleID.BULLET;

        var particle = SimpleParticleSpawner.Instance.particles[particleID].GetComponentInChildren<ParticleSystem>();
        var speed = particle.main.startSpeed.constant * particle.main.simulationSpeed;
        var lifeTime = b.objDist / speed;
        SimpleParticleSpawner.Instance.SpawnParticle(particle.gameObject, Owner.transform.position, Owner.transform.rotation, lifeTime);

        var muzzleFlashID = SimpleParticleSpawner.ParticleID.MUZZLEFLASH;
        var muzzleFlashParticle = SimpleParticleSpawner.Instance.particles[muzzleFlashID].GetComponentInChildren<ParticleSystem>();

        SimpleParticleSpawner.Instance.SpawnParticle(muzzleFlashParticle.gameObject, _muzzle.transform.position, Owner.transform.forward, Owner.transform);
    }
}

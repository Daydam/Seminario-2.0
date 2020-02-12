using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shotgun : TapWeapon
{
    public SO_ShotgunBase shotgunStatsData;

    public override void Shoot()
    {
        
        Owner.ApplyShake(ShakeDuration, ShakeIntensity);
        CalculatePelletsDispersion(weaponStatsData.falloffEnd);
    }

    void CalculatePelletsDispersion(float depth)
    {
        for (int i = 0; i < shotgunStatsData.pellets; i++)
        {
            var dispersionPoint = Random.Range(-shotgunStatsData.maxDispersionRadius, shotgunStatsData.maxDispersionRadius);
            var dir = Owner.transform.forward + (Owner.transform.right * dispersionPoint);

            var b = new HitscanBullet(Owner.transform.position, dir.normalized, Owner, _damageFalloff, _knockbackFalloff, shotgunStatsData.pellets);

            var bulletParticleID = SimpleParticleSpawner.ParticleID.BULLET;

            var bulletParticle = SimpleParticleSpawner.Instance.particles[bulletParticleID].GetComponentInChildren<ParticleSystem>();
            var speed = bulletParticle.main.startSpeed.constant * bulletParticle.main.simulationSpeed;
            var lifeTime = b.objDist / speed;

            SimpleParticleSpawner.Instance.SpawnParticle(bulletParticle.gameObject, Owner.transform.position, dir, lifeTime);
            
        }

        var muzzleFlashID = SimpleParticleSpawner.ParticleID.MUZZLE_FLASH;
        var muzzleFlashParticle = SimpleParticleSpawner.Instance.particles[muzzleFlashID].GetComponentInChildren<ParticleSystem>();

        SimpleParticleSpawner.Instance.SpawnParticle(muzzleFlashParticle.gameObject, _muzzle.transform.position, Owner.transform.forward, Owner.transform);
    }

}

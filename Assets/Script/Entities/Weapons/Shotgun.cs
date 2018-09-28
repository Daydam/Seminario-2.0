using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shotgun : TapWeapon
{
    public float maxDispersionRadius = 0.1f;
    public int pellets = 12;

    public override void Shoot()
    {
        Owner.ApplyVibration(0, VibrationIntensity, VibrationDuration);
        Owner.ApplyShake(ShakeDuration, ShakeIntensity);
        CalculatePelletsDispersion(falloffEnd);
    }

    void CalculatePelletsDispersion(float depth)
    {
        for (int i = 0; i < pellets; i++)
        {
            var dispersionPoint = Random.Range(-maxDispersionRadius, maxDispersionRadius);
            var dir = Owner.transform.forward + (Owner.transform.right * dispersionPoint);

            var b = new HitscanBullet(Owner.transform.position, dir.normalized, Owner, damageFalloff, knockbackFalloff, pellets);

            var bulletParticleID = SimpleParticleSpawner.ParticleID.BULLET;

            var bulletParticle = SimpleParticleSpawner.Instance.particles[bulletParticleID].GetComponentInChildren<ParticleSystem>();
            var speed = bulletParticle.main.startSpeed.constant * bulletParticle.main.simulationSpeed;
            var lifeTime = b.objDist / speed;

            SimpleParticleSpawner.Instance.SpawnParticle(bulletParticle.gameObject, Owner.transform.position, dir, lifeTime);
            
        }

        var muzzleFlashID = SimpleParticleSpawner.ParticleID.MUZZLEFLASH;
        var muzzleFlashParticle = SimpleParticleSpawner.Instance.particles[muzzleFlashID].GetComponentInChildren<ParticleSystem>();

        SimpleParticleSpawner.Instance.SpawnParticle(muzzleFlashParticle.gameObject, _muzzle.transform.position, Owner.transform.forward, Owner.transform);
    }

}

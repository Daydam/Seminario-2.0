using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : Weapon
{

    protected override void InitializeUseCondition()
    {
        _canUseWeapon = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (_control.MainWeapon() && _canUseWeapon())
        {
            PlaySound(shootSound);
            Shoot();
            _currentCooldown = _realCooldown;
        }
    }

    public override void Shoot()
    {
        Owner.ApplyVibration(0, VibrationIntensity, VibrationDuration);
        Owner.ApplyShake(ShakeDuration, ShakeIntensity);

        var b = new HitscanBullet(Owner.transform.position, Owner.transform.forward, Owner, _damageFalloff, _knockbackFalloff, 1);

        var particleID = SimpleParticleSpawner.ParticleID.BULLET;

        var particle = SimpleParticleSpawner.Instance.particles[particleID].GetComponentInChildren<ParticleSystem>();
        var speed = particle.main.startSpeed.constant * particle.main.simulationSpeed;
        var lifeTime = b.objDist / speed;
        SimpleParticleSpawner.Instance.SpawnParticle(particle.gameObject, Owner.transform.position, Owner.transform.rotation, lifeTime);

        var muzzleFlashID = SimpleParticleSpawner.ParticleID.MUZZLEFLASH;
        var muzzleFlashParticle = SimpleParticleSpawner.Instance.particles[muzzleFlashID].GetComponentInChildren<ParticleSystem>();

        SimpleParticleSpawner.Instance.SpawnParticle(muzzleFlashParticle.gameObject, _muzzle.transform.position, Owner.transform.forward, Owner.transform);
    }

    public override void PlaySound(AudioClip sound)
    {
        _audioSource.PlayOneShot(sound);
    }
}

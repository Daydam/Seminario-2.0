using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shotgun : TapWeapon
{
    public float maxDispersionRadius = 12;
    public float pellets = 12;

    public override void Shoot()
    {
        CalculatePelletsDispersion(falloffEnd);
    }

    void CalculatePelletsDispersion(float depth)
    {
        for (int i = 0; i < pellets; i++)
        {
            var dispersionPoint = Random.Range(0f, maxDispersionRadius);

            var deltaAngle = Mathf.Atan(depth / dispersionPoint);

            deltaAngle = i + 1 <= pellets / 2 ? deltaAngle *= -1 : deltaAngle;

            var vectorRotated = Quaternion.Euler(0, deltaAngle, 0) * Owner.transform.forward;

            var b = new HitscanBullet(Owner.transform.position, vectorRotated.normalized, Owner, damageFalloff, knockbackFalloff, pellets);
            b.SpawnParticle(particle, Owner.transform.position, Owner.transform.rotation * Quaternion.Euler(vectorRotated));
        }

    }

}

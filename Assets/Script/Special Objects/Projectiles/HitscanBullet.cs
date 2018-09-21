using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitscanBullet
{
    public float objDist;

    public HitscanBullet(Vector3 origin, Vector3 dir, Player player, AnimationCurve damage, AnimationCurve knockback, int inputPellets)
    {
        var pellets = Mathf.Max(1, inputPellets);

        var rch = new RaycastHit();
        var damagableMask = HitscanLayers.BlockerLayerMask()/*.SubstractFrom(player.gameObject.layer)*/;
        var hitDamagable = Physics.Raycast(origin, dir.normalized, out rch, 100, damagableMask);
        var col = rch.collider;
        var dist = rch.distance;
        objDist = dist;

        if (hitDamagable)
        {
            Debug.DrawRay(origin, dir * dist, Color.magenta, 3);

            var appliableDamage = GetCurveOutput(damage, objDist, pellets);
            var appliableKnockback = GetCurveOutput(damage, objDist, pellets);

            var plays = new int[] { LayerMask.NameToLayer("Player1"), LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Player3"), LayerMask.NameToLayer("Player4") };

            if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("DestructibleWall"), LayerMask.NameToLayer("DestructibleStructure"), LayerMask.NameToLayer("Shield")))
            {
                var structure = col.GetComponent(typeof(IDamageable)) as IDamageable;

                structure.TakeDamage(appliableDamage);
            }
            else if (col.gameObject.LayerDifferentFrom(player.gameObject.layer) && col.gameObject.LayerMatchesWith(plays))
            {

                var target = col.GetComponent<Player>();

                target.TakeDamage(appliableDamage, player.tag);

                var damageParticleID = SimpleParticleSpawner.ParticleID.DAMAGE;
                var damageParticle = SimpleParticleSpawner.Instance.particles[damageParticleID].GetComponentInChildren<ParticleSystem>();

                SimpleParticleSpawner.Instance.SpawnParticle(damageParticle.gameObject, rch.point, rch.normal);

                target.ApplyKnockback(knockback.Evaluate(dist) / pellets, dir.normalized, player);
            }
            else
            {
                //do stuff
            }
        }
        else
        {
            objDist = 125;
            Debug.DrawRay(origin, dir * objDist, Color.magenta, 3);
        }
    }

    public float GetCurveOutput(AnimationCurve curve, float distance, float pellets)
    {
        return curve.Evaluate(distance) / pellets;
    }

    public struct HitscanLayers
    {
        public static int DamagableLayerMask()
        {
            return LayerMask.GetMask("Player1", "Player2", "Player3", "Player4", "DestructibleWall", "DestructibleStructure");
        }

        public static int BlockerLayerMask()
        {
            return ~(LayerMask.GetMask("Unrenderizable", "Ignore Raycast", "EMPCloud", "DeathZone"));
        }
    }
}

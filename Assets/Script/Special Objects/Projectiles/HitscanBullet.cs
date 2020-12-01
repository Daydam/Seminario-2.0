using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PhoenixDevelopment;

public class HitscanBullet
{
    public float objDist;
    [Range(0f,1f)]
    public float wallHitVolume = 0.5f;

    public HitscanBullet(Vector3 origin, Vector3 dir, Player player, AnimationCurve damage, AnimationCurve knockback, int inputPellets)
    {
        var pellets = Mathf.Max(1, inputPellets);

        var rch = new RaycastHit();
        var damagableMask = HitscanLayers.BlockerLayerMask();
        var hitDamagable = Physics.Raycast(origin, dir.normalized, out rch, 100, damagableMask);
        var col = rch.collider;
        var dist = rch.distance;
        objDist = dist;

        var otherPlayerLayers = new int[] { LayerMask.NameToLayer("Player1"), LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Player3"), LayerMask.NameToLayer("Player4") }
                                            .Where(x => x != player.gameObject.layer)
                                            .ToArray();
        var otherPlayerTags = new string[] { "Player 1", "Player 2", "Player 3", "Player 4" }
                                             .Where(x => x != player.gameObject.tag)
                                             .ToArray();

        if (hitDamagable)
        {
            Debug.DrawRay(origin, dir * dist, Color.magenta, 3);

            var appliableDamage = GetCurveOutput(damage, objDist, pellets);
            var appliableKnockback = GetCurveOutput(damage, objDist, pellets);

            if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("DestructibleWall"), LayerMask.NameToLayer("DestructibleStructure"))
               || col.gameObject.TagMatchesWith("Shield")
               || col.gameObject.TagMatchesWith(otherPlayerTags)
               )
            {
                if (col.gameObject.LayerMatchesWith(otherPlayerLayers))
                {
                    if (col.GetComponent(typeof(IDamageBlocker)) as IDamageBlocker != null)
                    {
                        //do shit with the shield
                        return;
                    } 

                    var damageable = col.GetComponent(typeof(IDamageable)) as IDamageable;

                    var playerComponent = col.GetComponent<Player>();
                    if (playerComponent)
                    {
                        player.Stats.DamageDealt += appliableDamage;
                        playerComponent.Stats.DamageTaken += appliableDamage;
                        playerComponent.ApplyKnockback(knockback.Evaluate(dist) / pellets, dir.normalized, player);
                        var damageParticleID = SimpleParticleSpawner.ParticleID.DAMAGE;
                        var damageParticle = SimpleParticleSpawner.Instance.particles[damageParticleID].GetComponentInChildren<ParticleSystem>();

                        SimpleParticleSpawner.Instance.SpawnParticle(damageParticle.gameObject, rch.point, rch.normal);
                        playerComponent.TakeDamage(appliableDamage, player.tag, rch.point);
                    }
                    else
                    {
                        damageable.TakeDamage(appliableDamage, player.tag);
                    }
                }
                else
                {
                    var damageable = col.GetComponent(typeof(IDamageable)) as IDamageable;
                    if (damageable != null) damageable.TakeDamage(appliableDamage, rch.point);
                    

                    SimpleParticleSpawner.Instance.SpawnParticlesImpact(rch.point, Quaternion.LookRotation(rch.normal), 2.0f);

                    EventManager.Instance.DispatchEvent("SpawnDust", new object[] { rch.normal });

                    var wall = col.GetComponent<DestructibleBase>();
                    if (wall)
                    {
                        //I know, this random is super hardcoded, it's here just in case
                        wall.PlayBulletHitSound((GetCurveOutput(damage, objDist, pellets)*wallHitVolume)/GetCurveOutput(damage,0,pellets), Random.Range(0.7f,0.85f));
                    }
                }
            }
            else
            {
                SimpleParticleSpawner.Instance.SpawnParticlesImpact(rch.point, Quaternion.LookRotation(rch.normal), 2.0f);

                objDist = dist;
                Debug.DrawRay(origin, dir * objDist, Color.red, 3);
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

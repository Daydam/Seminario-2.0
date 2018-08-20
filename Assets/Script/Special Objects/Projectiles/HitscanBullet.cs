using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitscanBullet
{
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

    public HitscanBullet(Vector3 origin, Vector3 dir, Player player, AnimationCurve damage, AnimationCurve knockback)
    {
        var rch = new RaycastHit();
        var damagableMask = HitscanLayers.BlockerLayerMask()/*.SubstractFrom(player.gameObject.layer)*/;
        var hitDamagable = Physics.Raycast(origin, dir.normalized, out rch, 100, damagableMask);
        var col = rch.collider;
        var dist = rch.distance;

        if (hitDamagable)
        {
            Debug.DrawRay(origin, dir*dist, Color.magenta, 3);

            var plays = new int[] { LayerMask.NameToLayer("Player1"), LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Player3"), LayerMask.NameToLayer("Player4") };

            if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("DestructibleWall"), LayerMask.NameToLayer("DestructibleStructure")))
            {
                col.gameObject.SetActive(false);
            }
            else if (col.gameObject.LayerDifferentFrom(player.gameObject.layer) && col.gameObject.LayerMatchesWith(plays))
            {
                if (col.gameObject.TagMatchesWith("Shield"))
                {
                    return;
                }

                var target = col.GetComponent<Player>();

                target.TakeDamage(damage.Evaluate(dist), player.tag);

                target.ApplyKnockback(knockback.Evaluate(dist), dir.normalized, player);
            }
            else
            {
                //do stuff
            }
        }
    }
}

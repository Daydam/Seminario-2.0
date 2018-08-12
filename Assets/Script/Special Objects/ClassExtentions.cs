using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ClassExtentions
{
    public static bool TagDifferentFrom(this GameObject checker, params string[] tags)
    {
        return !tags.Where(x => x == checker.tag).Any();
    }

    public static bool LayerDifferentFrom(this GameObject checker, params int[] layers)
    {
        return !layers.Where(x => x == checker.layer).Any();
    }

    public static bool TagMatchesWith(this GameObject checker, params string[] tags)
    {
        return tags.Where(x => x == checker.tag).Any();
    }

    public static bool LayerMatchesWith(this GameObject checker, params int[] layers)
    {
        return layers.Where(x => x == checker.layer).Any();
    }

    public static bool ContainsLayer(this Camera cam, int layer)
    {
        var cull = cam.cullingMask;
        return cull == (cull | (1 << layer));
    }

    public static int MutateTo(this int mask, params int[] layers)
    {
        var ret = 1;

        var newMask = layers.Aggregate(ret, (acum, curr) => acum |= 1 << curr);

        return newMask;
    }

    public static Transform FindChildIn(this Transform trf, string name, bool includeInactive)
    {
        var ret = trf.GetComponentsInChildren<Transform>().Where(x => x.name == name);
        if (!includeInactive) ret = ret.Where(x => x.gameObject.activeInHierarchy);

        return ret.FirstOrDefault();
    }

    public static IEnumerable<Transform> FindChildrenIn(this Transform trf, string name, bool includeInactive)
    {
        var ret = trf.GetComponentsInChildren<Transform>().Where(x => x.name == name);
        if (!includeInactive) ret = ret.Where(x => x.gameObject.activeInHierarchy);

        return ret;
    }

    public static void AddMultipleKeys(this AnimationCurve curve, params Keyframe[] keys)
    {
        foreach (var key in keys)
        {
            curve.AddKey(key);
        }
    }

}

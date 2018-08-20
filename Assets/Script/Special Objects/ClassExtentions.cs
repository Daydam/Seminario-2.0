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
        var ret = layers.Take(1).First();

        var newMask = layers.Skip(1).Aggregate(ret, (acum, curr) => acum |= 1 << curr);

        return newMask;
    }

    public static int SubstractFrom(this int mask, params int[] layers)
    {
        var seed = layers.Take(1).First();
        var substract = layers.Skip(1).Aggregate(seed, (acum, curr) => acum |= 1 << curr);

        return mask | ~(1 << substract);

    }

    public static int MutateTo(this int mask, params string[] layers)
    {
        var transformed = layers.Select(x => LayerMask.NameToLayer(x));

        var seed = transformed.Take(1).First();

        var newMask = transformed.Skip(1).Aggregate(seed, (acum, curr) => acum |= 1 << curr);

        return newMask;
    }

    public static int SubstractFrom(this int mask, params string[] layers)
    {
        var transformed = layers.Select(x => LayerMask.NameToLayer(x));


        var seed = transformed.Take(1).First();
        var substract = transformed.Skip(1).Aggregate(seed, (acum, curr) => acum |= 1 << curr);

        return mask | ~(1 << substract);

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

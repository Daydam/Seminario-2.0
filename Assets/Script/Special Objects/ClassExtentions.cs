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
}

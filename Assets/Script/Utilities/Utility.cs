using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Random = UnityEngine.Random;
public enum Direction {
	Up		= 0,
	Right	= 1,
	Down	= 2,
	Left	= 3
}

public static class Utility {
	//											  U  R   D   L
	//											  N  E   S   W
	public static int[] xMoveVector = new int[] { 0, 1,  0, -1 };
	public static int[] yMoveVector = new int[] { 1, 0, -1,  0 };


	public static T Log<T>(T value, string prefix = "") {
		Debug.Log(prefix + value);
		return value;
	}

    public static IEnumerable<T> MyTakeWhile<T>(this IEnumerable<T> list, Func<T, bool> predicate)
    {
        foreach (var x in list)
        {
            if (!predicate(x))
                break;

            yield return x;
        }
    }

    public static IEnumerable<T> Generate<T>(T seed, Func<T, T> mutate)
    {
        var accum = seed;
        while (true)
        {
            yield return accum;
            accum = mutate(accum);
        }
    }

    /*public static Dictionary<K, V> Update<K,V>(this Dictionary<K, V> dict1, Dictionary<K, V> dict2)
    {
        var ret = new Dictionary<K, V>();
        foreach (var kv in dict1)
            ret[kv.Key] = kv.Value;
        foreach (var kv in dict2)
            ret[kv.Key] = kv.Value;
        return ret;
    }*/

    public static IEnumerable<T> TakeWhileInclusive<T>(this IEnumerable<T> list, Func<T, bool> predicate)
    {
        foreach (var x in list)
        {
            yield return x;

            if (!predicate(x))
                break;
        }
    }


    public static void KnuthShuffle<T>(List<T> array) {
		for(int i = 0; i<array.Count-1; i++) {
			var j = Random.Range(i, array.Count);
			if(i != j) {
				var temp = array[j];
				array[j] = array[i];
				array[i] = temp;
			}
		}
	}
}

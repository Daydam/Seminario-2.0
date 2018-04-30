

// File generated using Python 3
using System;
using System.Collections.Generic;

static class ZipExtension {
	
	//Zip 2
	static public IEnumerable<Dst> ZipWith<T1, T2, Dst>(this IEnumerable<T1> e1, IEnumerable<T2> e2, Func<T1, T2, Dst> map) {
		var result = new List<Dst>();
		var it1 = e1.GetEnumerator();
		var it2 = e2.GetEnumerator();
		while(it1.MoveNext() && it2.MoveNext())
			result.Add(map(it1.Current, it2.Current));
		return result;
	}

	static public IEnumerable<Tuple<T1, T2>> Zip<T1, T2>(this IEnumerable<T1> e1, IEnumerable<T2> e2) {
		return e1.ZipWith(e2, (v1, v2) => Tuple.Create(v1, v2));
	}


	//Zip 3
	static public IEnumerable<Dst> ZipWith<T1, T2, T3, Dst>(this IEnumerable<T1> e1, IEnumerable<T2> e2, IEnumerable<T3> e3, Func<T1, T2, T3, Dst> map) {
		var result = new List<Dst>();
		var it1 = e1.GetEnumerator();
		var it2 = e2.GetEnumerator();
		var it3 = e3.GetEnumerator();
		while(it1.MoveNext() && it2.MoveNext() && it3.MoveNext())
			result.Add(map(it1.Current, it2.Current, it3.Current));
		return result;
	}

	static public IEnumerable<Tuple<T1, T2, T3>> Zip<T1, T2, T3>(this IEnumerable<T1> e1, IEnumerable<T2> e2, IEnumerable<T3> e3) {
		return e1.ZipWith(e2, e3, (v1, v2, v3) => Tuple.Create(v1, v2, v3));
	}


	//Zip 4
	static public IEnumerable<Dst> ZipWith<T1, T2, T3, T4, Dst>(this IEnumerable<T1> e1, IEnumerable<T2> e2, IEnumerable<T3> e3, IEnumerable<T4> e4, Func<T1, T2, T3, T4, Dst> map) {
		var result = new List<Dst>();
		var it1 = e1.GetEnumerator();
		var it2 = e2.GetEnumerator();
		var it3 = e3.GetEnumerator();
		var it4 = e4.GetEnumerator();
		while(it1.MoveNext() && it2.MoveNext() && it3.MoveNext() && it4.MoveNext())
			result.Add(map(it1.Current, it2.Current, it3.Current, it4.Current));
		return result;
	}

	static public IEnumerable<Tuple<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(this IEnumerable<T1> e1, IEnumerable<T2> e2, IEnumerable<T3> e3, IEnumerable<T4> e4) {
		return e1.ZipWith(e2, e3, e4, (v1, v2, v3, v4) => Tuple.Create(v1, v2, v3, v4));
	}


}

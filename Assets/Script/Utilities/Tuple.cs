
// File generated using Python 3

using System;		//For Object.Equals


public class Tuple<T1> {
	public T1	Item1	{ get; private set; }
	internal Tuple(T1 item1)
	{
		Item1	= item1;
	}
	public bool Equals(Tuple<T1> rhs) {
		return Object.Equals(rhs.Item1, Item1);
	}
}

public class Tuple<T1, T2> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	internal Tuple(T1 item1, T2 item2)
	{
		Item1	= item1;
		Item2	= item2;
	}
	public bool Equals(Tuple<T1, T2> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2);
	}
}

public class Tuple<T1, T2, T3> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
	}
	public bool Equals(Tuple<T1, T2, T3> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3);
	}
}

public class Tuple<T1, T2, T3, T4> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
	}
	public bool Equals(Tuple<T1, T2, T3, T4> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4);
	}
}

public class Tuple<T1, T2, T3, T4, T5> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	public T12	Item12	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
		Item12	= item12;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11) && Object.Equals(rhs.Item12, Item12);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	public T12	Item12	{ get; private set; }
	public T13	Item13	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
		Item12	= item12;
		Item13	= item13;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11) && Object.Equals(rhs.Item12, Item12) && Object.Equals(rhs.Item13, Item13);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	public T12	Item12	{ get; private set; }
	public T13	Item13	{ get; private set; }
	public T14	Item14	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
		Item12	= item12;
		Item13	= item13;
		Item14	= item14;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11) && Object.Equals(rhs.Item12, Item12) && Object.Equals(rhs.Item13, Item13) && Object.Equals(rhs.Item14, Item14);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	public T12	Item12	{ get; private set; }
	public T13	Item13	{ get; private set; }
	public T14	Item14	{ get; private set; }
	public T15	Item15	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
		Item12	= item12;
		Item13	= item13;
		Item14	= item14;
		Item15	= item15;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11) && Object.Equals(rhs.Item12, Item12) && Object.Equals(rhs.Item13, Item13) && Object.Equals(rhs.Item14, Item14) && Object.Equals(rhs.Item15, Item15);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	public T12	Item12	{ get; private set; }
	public T13	Item13	{ get; private set; }
	public T14	Item14	{ get; private set; }
	public T15	Item15	{ get; private set; }
	public T16	Item16	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
		Item12	= item12;
		Item13	= item13;
		Item14	= item14;
		Item15	= item15;
		Item16	= item16;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11) && Object.Equals(rhs.Item12, Item12) && Object.Equals(rhs.Item13, Item13) && Object.Equals(rhs.Item14, Item14) && Object.Equals(rhs.Item15, Item15) && Object.Equals(rhs.Item16, Item16);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	public T12	Item12	{ get; private set; }
	public T13	Item13	{ get; private set; }
	public T14	Item14	{ get; private set; }
	public T15	Item15	{ get; private set; }
	public T16	Item16	{ get; private set; }
	public T17	Item17	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
		Item12	= item12;
		Item13	= item13;
		Item14	= item14;
		Item15	= item15;
		Item16	= item16;
		Item17	= item17;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11) && Object.Equals(rhs.Item12, Item12) && Object.Equals(rhs.Item13, Item13) && Object.Equals(rhs.Item14, Item14) && Object.Equals(rhs.Item15, Item15) && Object.Equals(rhs.Item16, Item16) && Object.Equals(rhs.Item17, Item17);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	public T12	Item12	{ get; private set; }
	public T13	Item13	{ get; private set; }
	public T14	Item14	{ get; private set; }
	public T15	Item15	{ get; private set; }
	public T16	Item16	{ get; private set; }
	public T17	Item17	{ get; private set; }
	public T18	Item18	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
		Item12	= item12;
		Item13	= item13;
		Item14	= item14;
		Item15	= item15;
		Item16	= item16;
		Item17	= item17;
		Item18	= item18;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11) && Object.Equals(rhs.Item12, Item12) && Object.Equals(rhs.Item13, Item13) && Object.Equals(rhs.Item14, Item14) && Object.Equals(rhs.Item15, Item15) && Object.Equals(rhs.Item16, Item16) && Object.Equals(rhs.Item17, Item17) && Object.Equals(rhs.Item18, Item18);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	public T12	Item12	{ get; private set; }
	public T13	Item13	{ get; private set; }
	public T14	Item14	{ get; private set; }
	public T15	Item15	{ get; private set; }
	public T16	Item16	{ get; private set; }
	public T17	Item17	{ get; private set; }
	public T18	Item18	{ get; private set; }
	public T19	Item19	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18, T19 item19)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
		Item12	= item12;
		Item13	= item13;
		Item14	= item14;
		Item15	= item15;
		Item16	= item16;
		Item17	= item17;
		Item18	= item18;
		Item19	= item19;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11) && Object.Equals(rhs.Item12, Item12) && Object.Equals(rhs.Item13, Item13) && Object.Equals(rhs.Item14, Item14) && Object.Equals(rhs.Item15, Item15) && Object.Equals(rhs.Item16, Item16) && Object.Equals(rhs.Item17, Item17) && Object.Equals(rhs.Item18, Item18) && Object.Equals(rhs.Item19, Item19);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> {
	public T1	Item1	{ get; private set; }
	public T2	Item2	{ get; private set; }
	public T3	Item3	{ get; private set; }
	public T4	Item4	{ get; private set; }
	public T5	Item5	{ get; private set; }
	public T6	Item6	{ get; private set; }
	public T7	Item7	{ get; private set; }
	public T8	Item8	{ get; private set; }
	public T9	Item9	{ get; private set; }
	public T10	Item10	{ get; private set; }
	public T11	Item11	{ get; private set; }
	public T12	Item12	{ get; private set; }
	public T13	Item13	{ get; private set; }
	public T14	Item14	{ get; private set; }
	public T15	Item15	{ get; private set; }
	public T16	Item16	{ get; private set; }
	public T17	Item17	{ get; private set; }
	public T18	Item18	{ get; private set; }
	public T19	Item19	{ get; private set; }
	public T20	Item20	{ get; private set; }
	internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18, T19 item19, T20 item20)
	{
		Item1	= item1;
		Item2	= item2;
		Item3	= item3;
		Item4	= item4;
		Item5	= item5;
		Item6	= item6;
		Item7	= item7;
		Item8	= item8;
		Item9	= item9;
		Item10	= item10;
		Item11	= item11;
		Item12	= item12;
		Item13	= item13;
		Item14	= item14;
		Item15	= item15;
		Item16	= item16;
		Item17	= item17;
		Item18	= item18;
		Item19	= item19;
		Item20	= item20;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> rhs) {
		return Object.Equals(rhs.Item1, Item1) && Object.Equals(rhs.Item2, Item2) && Object.Equals(rhs.Item3, Item3) && Object.Equals(rhs.Item4, Item4) && Object.Equals(rhs.Item5, Item5) && Object.Equals(rhs.Item6, Item6) && Object.Equals(rhs.Item7, Item7) && Object.Equals(rhs.Item8, Item8) && Object.Equals(rhs.Item9, Item9) && Object.Equals(rhs.Item10, Item10) && Object.Equals(rhs.Item11, Item11) && Object.Equals(rhs.Item12, Item12) && Object.Equals(rhs.Item13, Item13) && Object.Equals(rhs.Item14, Item14) && Object.Equals(rhs.Item15, Item15) && Object.Equals(rhs.Item16, Item16) && Object.Equals(rhs.Item17, Item17) && Object.Equals(rhs.Item18, Item18) && Object.Equals(rhs.Item19, Item19) && Object.Equals(rhs.Item20, Item20);
	}
}


public static class Tuple {
	
	public static Tuple<T1> Create<T1>(T1 item1)
	{
		return new Tuple<T1>(item1);
	}

	public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
	{
		return new Tuple<T1, T2>(item1, item2);
	}

	public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
	{
		return new Tuple<T1, T2, T3>(item1, item2, item3);
	}

	public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
	{
		return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
	}

	public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
	{
		return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7, item8);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(item1, item2, item3, item4, item5, item6, item7, item8, item9);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, item17);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, item17, item18);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18, T19 item19)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, item17, item18, item19);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18, T19 item19, T20 item20)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, item17, item18, item19, item20);
	}

}

/********************************************************************
	Framework Script
	Class: 	GreyFramework::Pair
	Author:	
	Created:	
	Note:	$end$
*********************************************************************/



namespace GreyFramework
{
    public class Pair<K, V>
    {
        public K Key;
        public V Value;

        public Pair() { }
        public Pair(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", Key.ToString(), Value.ToString());
        }
    }


    public class Pair3<V1, V2 , V3>
    {
        public V1 Value1;        public V2 Value2;        public V3 Value3;
        public Pair3( ) { }
        public Pair3( V1 v1 , V2 v2 , V3 v3 )
        {
            Value1 = v1;    Value2 = v2;    Value3 = v3;
        }

        public override string ToString( )
        {
            return string.Format( "{0},{1},{2}" , Value1.ToString() , Value2.ToString() , Value3.ToString() );
        }
    }

    public class Pair4<V1, V2, V3, V4>
    {
        public V1 Value1; public V2 Value2; public V3 Value3; public V4 Value4;
        public Pair4( ) { }
        public Pair4( V1 v1 , V2 v2 , V3 v3 , V4 v4 )
        {
            Value1 = v1; Value2 = v2; Value3 = v3; Value4 = v4;
        }

        public override string ToString( )
        {
            return string.Format( "{0},{1},{2},{3}" , Value1.ToString() , Value2.ToString() , Value3.ToString() , Value4.ToString() );
        }
    }

    public class Pair5<V1, V2, V3, V4, V5>
    {
        public V1 Value1; public V2 Value2; public V3 Value3; public V4 Value4; public V5 Value5;
        public Pair5( ) { }
        public Pair5( V1 v1 , V2 v2 , V3 v3 , V4 v4 , V5 v5 )
        {
            Value1 = v1; Value2 = v2; Value3 = v3; Value4 = v4; Value5 = v5;
        }

        public override string ToString( )
        {
            return string.Format( "{0},{1},{2},{3},{4}" , Value1.ToString() , Value2.ToString() , Value3.ToString() , Value4.ToString() , Value5.ToString() );
        }
    }

    [System.Serializable]
    public class CVector2 : Pair<float , float> { }
    [System.Serializable]
    public class CVector3 : Pair3<float , float , float> { }
    [System.Serializable]
    public class CVector4 : Pair4<float , float , float , float> { }

    [System.Serializable]
    public class CColor32 : Pair4<byte , byte , byte , byte> { }

    [System.Serializable]
    public class CKeyframe : Pair5<float , float , int , float , float> { }

    [System.Serializable]
    public class CString2 : Pair<string , string> { }


}
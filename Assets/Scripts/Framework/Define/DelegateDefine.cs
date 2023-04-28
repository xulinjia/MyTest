

namespace GreyFramework
{
    using Google.Protobuf;

    public delegate void CallBack();
    public delegate void CallBack<T>(T _call1);
    public delegate void CallBack<T, T1>(T _call1, T1 _call2);
    public delegate void CallBack<T, T1, T2>(T _call1, T1 _call2, T2 _call3);
    public delegate void CallBack<T, T1, T2, T3>(T _call1, T1 _call2, T2 _call3, T3 _call4);
    public delegate void CallBack<T, T1, T2, T3, T4>(T _call1, T1 _call2, T2 _call3, T3 _call4, T4 _call5);
    public delegate R CallBackAndReturn<R>();
    public delegate R CallBackAndReturn<T, R>(T _call1);
    public delegate R CallBackAndReturn<T1, T2, R>(T1 _call1, T2 _call2);
    public delegate R CallBackAndReturn<T1, T2, T3, R>(T1 _call1, T2 _call2, T3 _call3);

    public delegate void CallBackRef<T>(ref T _call1);
    public delegate void CallBackRef<T, T1>(ref T _call1, ref T1 _call2);
    public delegate void CallBackRef<T, T1, T2>(ref T _call1, ref T1 _call2, ref T2 _call3);
    public delegate void CallBackRef<T, T1, T2, T3>(ref T _call1, ref T1 _call2, ref T2 _call3, ref T3 _call4);
    public delegate R CallBackRefAndReturn<T, R>(ref T _call1);
    public delegate R CallBackRefAndReturn<T, T1, R>(ref T _call1, ref T1 _call2);
    ///

    public delegate void OnAllocBuffer(out byte[] buffer, long len);
    public delegate void OnFreeBuffer(ref byte[] buffer);

}

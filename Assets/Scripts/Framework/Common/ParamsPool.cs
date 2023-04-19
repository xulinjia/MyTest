using System.Collections.Generic;
using System;
namespace GreyFramework
{
    ///仅限同步接口使用
    public class ParamsPool:Singleton<ParamsPool>
    {
        
        public object[] GetParams(object a)
        {
            var obj = _pool[1];
            obj[0] = a;
            return obj;
        }
        public object[] GetParams(object a,object b)
        {
            var obj = _pool[2];
            obj[0] = a;
            obj[1] = b;
            return obj;
        }
        public object[] GetParams(params object[] pars)
        {
            if (pars == null || pars.Length > 32)
                return null;
            int index = pars.Length;
            var obj = _pool[index];
            for (int i = 0; i < pars.Length; i++)
            {
                obj[i] = pars[i];
            }
            return obj;
        }
        public void Init(){
            _pool = new Dictionary<int, object[]>();
            for (int i = 0; i < max; i++)
            {
                _pool[i] = new object[i];   
            }
        }
        Dictionary<int,object[]> _pool  = null;
        int max = 32;
    }
}
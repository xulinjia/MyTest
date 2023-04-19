using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErisGame
{
    public enum eManager
    {
        Net,
        Httper,
        Resource,
        Event,
        Count,
    }
    public sealed class ManagerDefine
    {
        public static Manager CreateInstance(eManager type)
        {
            switch (type)
            {
                case eManager.Net:
                    return new NetManager();
                case eManager.Resource:
                    return new ResourceManager();
                case eManager.Httper:
                    return new HttperManager();
                case eManager.Event:
                    return new EventThreadManager();
                default:
                    break;
            }
            return null;
        }


    }
}

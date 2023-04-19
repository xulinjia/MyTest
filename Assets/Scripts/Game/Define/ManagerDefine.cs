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
        Resource,
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
                default:
                    break;
            }
            return null;
        }


    }
}

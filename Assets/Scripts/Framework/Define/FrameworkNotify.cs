using System;
using UnityEngine;

namespace GreyFramework
{
    public enum eHRESULT : int
    {
        Unknow = -1,
        /// <summary>
        /// 失败
        /// </summary>
        Failed = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 1,
        /// <summary>
        /// 完成
        /// </summary>
        Completed,
        /// <summary>
        /// 参数不健全
        /// </summary>
        MissParameters,
        Count,
    }
    static public class FrameworkNotify
    {
        const string INFO = "未定义通知";
        static Type CodeType = typeof(eHRESULT);

        static readonly eHRESULT[] Code = new eHRESULT[]
        {
            eHRESULT.Unknow,
            eHRESULT.Failed,
            eHRESULT.Succeed,
            eHRESULT.Completed,
            eHRESULT.MissParameters,
        };

        static readonly string[] Desc = new string[]
        {
            "未知错误",
            "错误",
            "成功",
            "完成",
            "参数缺失",
        };

        static public string GetDesc(eHRESULT code)
        {
            for (int i = 0; i < Code.Length; i++)
            {
                if (Code[i] == code)
                {
                    return Desc[i];
                }
            }

            return INFO;
        }

        static public void Printf(eHRESULT eCode)
        {
            Debug.Log(string.Format("[HRESULT_{0}] : {1}", eCode.ToString(), GetDesc(eCode)));
        }
    }
}

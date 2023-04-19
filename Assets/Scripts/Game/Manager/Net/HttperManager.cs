using GreyFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErisGame
{
    public class HttperManager : Manager
    {
        private List<Httper> httpers = new List<Httper>();
        public override eManager Type => eManager.Httper;
        /// <summary>
        /// Ïàµ±ÓÚAwake
        /// </summary>
        /// <returns></returns>
        public override eHRESULT OnInitialization()
        {
            return eHRESULT.Succeed;
        }

        public void AddHttper(Httper httper)
        {
            httpers.Add(httper);
        }

        protected override void OnLateUpdate()
        {
            if (httpers.Count == 0)
            {
                return;
            }
            for (int i = httpers.Count - 1; i >= 0; i--)
            {
                if (httpers[i].isDone)
                {
                    httpers[i].Dispose();
                    httpers.RemoveAt(i);
                }
            }

        }
    }
}


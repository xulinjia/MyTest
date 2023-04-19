using UnityEngine;
using System.Collections;

namespace TklGame
{
    public class EnumHome
    {

        public enum GameEvent
        {
            ChangeGameState,
            DownFaile,
            ResourcesComplete,
            ParseVersionFaile,//解析文件失败
            DownFaile_PlayDownload,
            PlayDownload_Complete,
            Net_Logic_Connect_Result,
            Net_Battle_Connect_Result,
            Net_DiConnect,
            Net_Logic_ReConnect_Result,

        }

        public enum UpdateState
        {
            /// <summary>
            /// 下载中
            /// </summary>
            Downloading = 1,
            /// <summary>
            /// 解压中
            /// </summary>
            UnCompressing = 2,
            /// <summary>
            /// 完成
            /// </summary>
            Complete = 3,
        }
    }
}

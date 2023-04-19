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
            ParseVersionFaile,//�����ļ�ʧ��
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
            /// ������
            /// </summary>
            Downloading = 1,
            /// <summary>
            /// ��ѹ��
            /// </summary>
            UnCompressing = 2,
            /// <summary>
            /// ���
            /// </summary>
            Complete = 3,
        }
    }
}

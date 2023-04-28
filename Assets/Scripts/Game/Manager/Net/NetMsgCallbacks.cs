using CenterMsg;
using Google.Protobuf;
using Google.Protobuf.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ErisGame
{
    public static class  NetMsgCallbacks
    {
        //消息处理
        //消息ID，服务器序列号，客户端序列号
        public static void MessageDispatchAsync(NetMsgID msgId, byte[] msgData)
        {
            Managers.GetNetManager().RemoveSendCache(msgId);
            //string s = "CenterMsg.C2G_Login";
            //Type t = System.Type.GetType(s,false,true);
            //Debug.LogError(t.FullName);
            //Debug.LogError(Deserialize<t>(msgData));


            if (msgId == NetMsgID.G2C_GMResponse)
            {
                G2C_GMResponse message = SerializaBuffer.Deserialize<G2C_GMResponse>(msgData);
                Debug.LogError("G2C_GMResponse，gm消息接收");
                Debug.LogError(message);
            }
            else if (msgId == NetMsgID.G2C_HeartBeat)
            {

                G2C_HeartBeat message = SerializaBuffer.Deserialize<G2C_HeartBeat>(msgData);
                if (message.SrvTimeTs == 0)
                {
                    Debug.LogError("G2C_HeartBeat心跳包接收！");
                }
            }
            else if (msgId == NetMsgID.G2C_OfflineNTfRes)
            {

                G2C_OfflineNTfRes message = SerializaBuffer.Deserialize<G2C_OfflineNTfRes>(msgData);
                if (message.Err == 0)
                {
                    Debug.LogError(msgData);
                }
            }
            else if (msgId == NetMsgID.G2C_Login)
            {
                G2C_Login message = SerializaBuffer.Deserialize<G2C_Login>(msgData);
                //Debug.LogError("G2C_Login");
                Debug.LogError(message);
                if (message.CreatedPlayer.Count == 0)
                {
                    Managers.GetNetManager().SendMessageAsync(NetMsgID.C2G_CreatePlayer,
                        new C2G_CreatePlayer() { AccountId = message.AccountId, 
                            PlayerName = message.AccountId, MainPlayerID = 1 });
                }
                else
                {
                    Managers.GetNetManager().SendMessageAsync(NetMsgID.C2G_PlayerLogin,
                        new C2G_PlayerLogin()
                        {
                            PlayerId = message.CreatedPlayer[0].PlayerId,
                            AccountId = message.AccountId,
                            LoginKey = message.LoginKey,
                        });
                }
            }
            else if (msgId == NetMsgID.G2C_CreatePlayer)
            {
                //Debug.LogError("G2C_CreatePlayer");
                G2C_CreatePlayer message = SerializaBuffer.Deserialize<G2C_CreatePlayer>(msgData);
                Debug.LogError(message);
                if (message.Err == 0)
                {
                    Debug.Log("创角成功");
                }
            }
            else if (msgId == NetMsgID.G2C_PlayerLogin)
            {
                //Debug.LogError("G2C_PlayerLogin");
                G2C_PlayerLogin message = SerializaBuffer.Deserialize<G2C_PlayerLogin>(msgData);
                Debug.LogError(message);
                if (message.Err == 0)
                {
                    Debug.Log("登录成功");
                    Managers.GetNetManager().StartHeartBeat();
                }

            }
            else if (msgId == NetMsgID.G2C_SyncPlayerDisplayInfo)
            {
                G2C_SyncPlayerDisplayInfo message = SerializaBuffer.Deserialize<G2C_SyncPlayerDisplayInfo>(msgData);
                Debug.LogError(message);
            }

            else if (msgId == NetMsgID.G2C_GetBagInfoRes) 
            {
                G2C_GetBagInfoRes message = SerializaBuffer.Deserialize<G2C_GetBagInfoRes>(msgData);
                Debug.LogError(message);
            }
            else if (msgId == NetMsgID.G2C_PlayerDataSyncFinish) 
            { 
                Debug.LogError("G2C_PlayerDataSyncFinish接收");
            }
            Managers.GetNetManager().SendNextMsg();
        }
    }
}

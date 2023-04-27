

using System.Collections.Generic;
public enum NetMsgID {
    C2G_Login = 100 ,
    C2G_HeartBeat = 101 ,
    C2G_CreatePlayer = 102 ,
    C2G_PlayerLogin = 103 ,
    C2G_ReconnectReq = 170 ,
    C2G_ChatMsgReq = 224 ,
    C2G_GMRequest = 225 ,
    G2C_Login = 500 ,
    G2C_HeartBeat = 501 ,
    G2C_CreatePlayer = 502 ,
    G2C_PlayerLogin = 503 ,
    G2C_GetBagInfoRes = 504 ,
    G2C_SyncPlayerDisplayInfo = 506 ,
    G2C_PlayerDataSyncFinish = 507 ,
    G2C_OfflineNTfRes = 508 ,
    G2C_GMResponse = 509 ,
    G2C_ReconnectRes = 577 ,
    G2C_ChatMsgRes = 632 ,
    G2C_PlayerDataReconnectSyncFinish = 906 ,

};




public static class MessageDefine {
    public static Dictionary<NetMsgID, NetMsgID> ReqAndRes = new Dictionary<NetMsgID, NetMsgID>(){
     { NetMsgID.C2G_ChatMsgReq , NetMsgID.G2C_ChatMsgRes },
     { NetMsgID.C2G_CreatePlayer , NetMsgID.G2C_CreatePlayer },
     { NetMsgID.C2G_GMRequest , NetMsgID.G2C_GMResponse },
     { NetMsgID.C2G_Login , NetMsgID.G2C_Login },
     { NetMsgID.C2G_PlayerLogin , NetMsgID.G2C_PlayerLogin },
     { NetMsgID.C2G_ReconnectReq , NetMsgID.G2C_ReconnectRes },

    };
}

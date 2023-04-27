
using GreyFramework;
using System;
using System.Collections;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using Google.Protobuf;
using CenterMsg;

namespace ErisGame
{
    public class MessageData
    {
        public NetMsgID packetId;//消息头部ID
        public int msgNum_server;//标识消息序列号(服务器)
        public int msgNum_client;//标识消息序列号(客户端)
        public byte[] data = new byte[1024];//解析数据
        public void Reset()
        {
            packetId = default;
            data = new byte[1024];
        }
    }
    /// <summary>
    /// 发起消息缓存队列数据
    /// </summary>
    public class SendMsgCacheQueueData
    {
        public NetMsgID msgId;
        public byte[] datas;
        public bool isWaitReplay = false;//是否等待回复
        public int msgNum;//客户端序列号
    }
    #region 辅助类
    public class StateObject
    {
        public MessageData messageData = new MessageData();

        public ushort bodyLength; //标识消息体数据长度
        public bool isParsedBody = false;////是否解析过消息体

        public bool isPareseMsgId = false; //是否解析过消息头
        public bool isParsedBodyLength = false;////是否解析过消息体长度
        public bool isPareseMsgNum_Server = false; //是否解析过消息头
        public bool isPareseMsgNum_Client = false; //是否解析过消息头

        public List<byte> dataBuffer_head = new List<byte>(); //消息头缓冲区 8个字节
        public List<byte> dataBuffer_data = new List<byte>(); //保存异步接收的数据。可能会超过1024设定的缓冲区大小
        public List<byte> buffer_total = new List<byte>();
        public bool isParsedHead = false;
        public void UnDecodeHeadData(int length_data, int length_id, int length_num_server, int length_num_client)
        {
            byte[] bt_length = new byte[length_data];
            byte[] bt_msgid = new byte[length_id];
            byte[] bt_num_server = new byte[length_num_server];
            byte[] bt_num_client = new byte[length_num_client];
            Array.Copy(buffer_total.ToArray(), 0, bt_length, 0, length_data);
            Array.Copy(buffer_total.ToArray(), 2, bt_msgid, 0, length_id);
            Array.Copy(buffer_total.ToArray(), 4, bt_num_server, 0, length_num_server);
            Array.Copy(buffer_total.ToArray(), 8, bt_num_client, 0, length_num_client);
            PaseMsgHeadId(bt_msgid);
            PaseMsgBodyLenth(bt_length);
            PaseMsgNum_Server(bt_num_server);
            PaseMsgNum_Client(bt_num_client);
            isParsedHead = true;

        }
        public void UnDecodeMsgBody()
        {
            //data = SerializaBuffer.BytesToString(bytes);
            messageData.data = new byte[bodyLength];
            for (int i = 0; i < bodyLength; i++)
            {
                messageData.data[i] = buffer_total[i + Managers.GetNetManager().msgHeadTotalLeng];
            }
            isParsedBody = true;
        }
        public void DispatchMsg()
        {
            //Debug.LogFormat("接收一个消息包完成:{0}", headId);
            lock (Managers.GetNetManager().reciveMsgs)
            {
                Managers.GetNetManager().reciveMsgs.Enqueue(messageData);
            }
        }
        public void PaseMsgHeadId(byte[] bytes)
        {
            messageData.packetId = (NetMsgID)System.Net.IPAddress.NetworkToHostOrder(SerializaBuffer.BytesToShort(bytes));
            isPareseMsgId = true;
        }
        public void PaseMsgBodyLenth(byte[] bytes)
        {
            short bodyLength_short = (short)BitConverter.ToUInt16(bytes, 0);
            bodyLength = (ushort)System.Net.IPAddress.NetworkToHostOrder(bodyLength_short);
            isParsedBodyLength = true;
        }
        /// <summary>
        /// 解析消息序列号 4个字节
        /// </summary>
        /// <param name="bytes"></param>
        public void PaseMsgNum_Server(byte[] bytes)
        {
            messageData.msgNum_server = System.Net.IPAddress.NetworkToHostOrder(SerializaBuffer.BytesToInt32(bytes));
            isPareseMsgNum_Server = true;
        }
        public void PaseMsgNum_Client(byte[] bytes)
        {
            messageData.msgNum_client = System.Net.IPAddress.NetworkToHostOrder(SerializaBuffer.BytesToInt32(bytes));
            isPareseMsgNum_Client = true;
        }
        public void PaseMsgBody(byte[] bytes)
        {
            //data = SerializaBuffer.BytesToString(bytes);
            messageData.data = bytes;
            isParsedBody = true;
        }
        public void AddBuffer(byte[] bytes)
        {
            buffer_total.AddRange(bytes);
        }
        public void Clean()
        {
            //移除上个数据头
            buffer_total.RemoveRange(0, bodyLength + Managers.GetNetManager().msgHeadTotalLeng);
            isPareseMsgId = false;
            isParsedBody = false;
            isParsedHead = false;
            isPareseMsgNum_Client = false;
            isPareseMsgNum_Server = false;
           
            
            isParsedBodyLength = false;
            bodyLength = 0;
            dataBuffer_head.Clear();
            dataBuffer_data.Clear();
        }
    }
    #endregion 辅助类
    public class NetManager : Manager
    {
        public Queue<MessageData> reciveMsgs = new Queue<MessageData>();
        public readonly int msgHeadLeng = sizeof(Int16); //消息内容长度
        public readonly int msgHeadID = sizeof(Int16); //消息头id
        public readonly int msgHeadNumServer = sizeof(UInt32); //消息编号  服务端序列号
        public readonly int msgHeadNumClient = sizeof(UInt32); //消息编号  客户端序列号
        public readonly int msgHeadTotalLeng = 12; //消息头总长度
        public override eManager Type => eManager.Net;

        public override eHRESULT OnInitialization() { return eHRESULT.Succeed; }

        public override IEnumerator OnCreate()
        {
            return base.OnCreate();
        }
        public void Dispose()
        {
            reciveMsgs.Clear();
            logic_sendCache.Clear();
        }

        protected override void OnUpdate()
        {
            lock (reciveMsgs)
            {
                int count = reciveMsgs.Count;
                for (int i = 0; i < count; i++)
                {
                    MessageData message = reciveMsgs.Dequeue();//队列第一个添加的数据
                    MessageDispatch(message.packetId, message.data);
                }
            }
        }
        public async Task TryConnectAsync(string ip, int post)
        {
            try
            {
                reciveMsgs.Clear();
                logic_sendCache.Clear();
                serverUri = new Uri("ws://43.143.36.51:8555");
                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(serverUri, CancellationToken.None);
                if (webSocket.State == WebSocketState.Open)
                {
                    Debug.LogError("连接成功！");
                    //startHeartBeat();
                    await ReceiveMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        //注册消息
        //public void RegCallbackFuncT<T>(NetMsgID msgId, object obj, NetMsgCallbackFuncT<T> msgCallback) where T : IMessage, new()
        //{
        //    Dictionary<object, ArrayList> callbackDic;
        //    ArrayList arrayList;
        //    if (recCallBacks.TryGetValue(msgId, out callbackDic))
        //    {
        //        if (callbackDic.TryGetValue(obj,out arrayList))
        //        {
        //            if (!arrayList.Contains(msgCallback))
        //            {
        //                //已经存在相同回调，直接返回
        //                return;
        //            }
        //            arrayList.Add(msgCallback);
        //        }
        //        else
        //        {
        //            arrayList = new ArrayList();
        //            arrayList.Add(msgCallback);
        //        }
        //    }
        //    else
        //    {
        //        callbackDic = new Dictionary<object, ArrayList>();
        //        arrayList = new ArrayList();
        //        arrayList.Add(msgCallback);
        //        callbackDic.Add(obj, arrayList);
        //        recCallBacks.Add(msgId, callbackDic);
        //    }
        //}
        ////public void RegCallbackFunc(NetMsgID msgId, object obj, NetMsgCallbackFunc msgCallback)
        ////注销消息
        //public void UnRegCallbackFuncT<T>(NetMsgID msgId, object obj, NetMsgCallbackFuncT<T> msgCallback) where T : IMessage, new()
        //{
        //    Dictionary<object, ArrayList> callbackDic;
        //    ArrayList arrayList;
        //    if (recCallBacks.TryGetValue(msgId, out callbackDic))
        //    {
        //        if (callbackDic.TryGetValue(obj,out arrayList))
        //        {
        //            if (arrayList.Contains(msgCallback))
        //            {
        //                arrayList.Remove(msgCallback);
        //            }
        //        }
        //    }

        //}
        ////注销对象所有消息
        //public void UnRegAllCallbackFunc(object obj)
        //{
        //    ArrayList arrayList;
        //    foreach (var item in recCallBacks)
        //    {
        //        if (item.Value.TryGetValue(obj,out arrayList))
        //        {
        //            item.Value.Remove(obj);
        //        }
        //    }
        //}
        //发送消息
        public async Task SendMessageAsync(NetMsgID netMsgId)
        {
            await SendMessageAsync(netMsgId,null);
        }
        public async Task SendMessageAsync(NetMsgID netMsgId, IMessage msgData)
        {

            short short_msgId = (short)netMsgId;
            byte[] byte_msgData = SerializaBuffer.Serialize(msgData);
            int dataLength = byte_msgData.Length;
            short dataLength_NO = System.Net.IPAddress.HostToNetworkOrder((short)byte_msgData.Length);
            if (dataLength > sendMaxByteCount)
            {
                Debug.LogError("发送的消息长度超出上限:" + dataLength);
                return;
            }
            sendBuffer.Clear();
            sendBuffer.AddRange(SerializaBuffer.ShortToBytes((short)dataLength_NO));
            //填充消息id
            short msgId_NO = System.Net.IPAddress.HostToNetworkOrder((short)short_msgId);
            sendBuffer.AddRange(SerializaBuffer.ShortToBytes(msgId_NO));
            //服务器序列号
            sendBuffer.AddRange(SerializaBuffer.Int32ToBytes(System.Net.IPAddress.HostToNetworkOrder(serverMsgNum)));
            //客户端序列号
            sendBuffer.AddRange(SerializaBuffer.Int32ToBytes(System.Net.IPAddress.HostToNetworkOrder(sendMsgNum)));
            //消息体数据
            sendBuffer.AddRange(byte_msgData);
            Debug.LogError(string.Format("{0}:::{1}",netMsgId,msgData));
            await sendMessageAsync(netMsgId, sendBuffer.ToArray(), sendMsgNum); ;
        }

        private void startHeartBeat()
        {
            Debug.Log("心跳:" + enableHeartBeat);
            if (enableHeartBeat)
            {
                if (thread_heart == null)
                {
                    thread_heart = new Thread(CheckHeart);
                    thread_heart.Start();
                }
                else
                {
                    Debug.Log("心跳已启动！");
                }
            }
        }
        private void CheckHeart()
        {
            while (true)
            {
                if (enableHeartBeat)
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        SendMessageAsync(heartMsgId);
                        Thread.Sleep(heartTime);
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
                else
                {
                    Thread.Sleep(1);

                }
            }
        }

        private async Task sendMessageAsync(NetMsgID msgId, byte[] datas, int msgNum)
        {
            //保存到发送队列中，依次发送
            SendMsgCacheQueueData cache = new SendMsgCacheQueueData();
            cache.datas = datas;
            cache.msgId = msgId;
            cache.msgNum = msgNum;
            logic_sendCache.Add(cache);
            await _sendMessageAsync();
        }
        private async Task _sendMessageAsync()
        {
            if (logic_sendCache.Count <= 0)
            {
                return;
            }
            SendMsgCacheQueueData data = null;
            for (int i = 0; i < logic_sendCache.Count; i++)
            {
                if (!logic_sendCache[i].isWaitReplay)
                {
                    data = logic_sendCache[i];
                    logic_sendCache[i].isWaitReplay = true;
                    break;
                }
            }
            byte[] message = data.datas;
            if (webSocket.State != WebSocketState.Open)
            {
                //await webSocket.ConnectAsync(serverUri, CancellationToken.None);
            }
            else if ((webSocket.State == WebSocketState.Open))
            {
                try
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Binary, true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("发送失败 msgId: {0}\n{1}", data.msgId, ex.Message);
                    return;
                }
            }
        }
        //消息处理
        //消息ID，服务器序列号，客户端序列号
        public void MessageDispatch(NetMsgID msgId, byte[] msgData)
        {
            RemoveSendCache(msgId);
            //string s = "CenterMsg.C2G_Login";
            //Type t = System.Type.GetType(s,false,true);
            //Debug.LogError(t.FullName);
            //Debug.LogError(Deserialize<t>(msgData));
            IMessage message;
            if (msgId == NetMsgID.G2C_Login)
            {
                message = SerializaBuffer.Deserialize<G2C_Login>(msgData);
                Debug.LogError(message);
                //startHeartBeat();
            }
            if (msgId == NetMsgID.G2C_CreatePlayer)
            {
                message = SerializaBuffer.Deserialize<G2C_CreatePlayer>(msgData);
                Debug.LogError(message);
                //startHeartBeat();
            }
            if (msgId == NetMsgID.G2C_HeartBeat)
            {
                message = SerializaBuffer.Deserialize<G2C_HeartBeat>(msgData);
                Debug.LogError(message);
            }
            SendNextMsg();
        }

        private async Task ReceiveMessage()
        {
            while (true)
            {
                Debug.LogError("ReceiveMessage");
                //网络断开时，跳出循环
                if (webSocket.State != WebSocketState.Open)
                {
                    Debug.LogError("网络已断开");
                    return;
                }
                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(bytesReceived, CancellationToken.None);
                //消息为结束消息是，断开网络并跳出循环
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Debug.LogError("网络已断开");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    return;
                }
                StateObject state = new StateObject();
                int numberOfBytesRead = bytesReceived.Count;
                if (numberOfBytesRead > 0)
                {
                    byte[] bytes = new byte[numberOfBytesRead];
                    Array.Copy(bytesReceived.ToArray(), 0, bytes, 0, numberOfBytesRead);
                    state.AddBuffer(bytes);
                    if (state.buffer_total.Count >= msgHeadTotalLeng && !state.isParsedHead)
                    {
                        state.UnDecodeHeadData(msgHeadID, msgHeadNumServer, msgHeadNumClient, msgHeadTotalLeng);
                    }
                    if (state.buffer_total.Count >= msgHeadTotalLeng + state.bodyLength && !state.isParsedBody)
                    {
                        state.UnDecodeMsgBody();
                        state.DispatchMsg();
                        state.Clean();
                    }
                }
            }
        }
        public void RemoveSendCache(NetMsgID netMsgID)
        {
            for (int i = 0; i < logic_sendCache.Count; i++)
            {
                NetMsgID resMsgID;
                if (MessageDefine.ReqAndRes.TryGetValue(logic_sendCache[i].msgId, out resMsgID))
                {
                    if (logic_sendCache[i].isWaitReplay == true && netMsgID == resMsgID)
                    {
                        logic_sendCache.Remove(logic_sendCache[i]);
                        break;
                    }
                }
            }
        }
        public void SendNextMsg()
        {
            bool isExist = false;
            for (int i = 0; i < logic_sendCache.Count; i++)
            {
                if (!logic_sendCache[i].isWaitReplay)
                {
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                return;
            }
            _sendMessageAsync().Wait();
        }

        private Uri serverUri;
        private static ClientWebSocket webSocket;

        //心跳相关
        private Thread thread_heart;
        private bool enableHeartBeat = true;
        private int heartTime = 3000;
        private NetMsgID heartMsgId = NetMsgID.C2G_HeartBeat;


        //发送消息最大长度 不包含包头
        private int sendMaxByteCount = 8192;
        private List<byte> sendBuffer = new List<byte>();
        private int serverMsgNum = 1;//服务器下发的 消息序列号
        private int sendMsgNum = 10000; //发送消息序列号
        private List<SendMsgCacheQueueData> logic_sendCache = new List<SendMsgCacheQueueData>();

        private bool isStopReadBuffer = false;

        //消息回调字典，按NetMsgID为索引，保存对应对象的回调列表
        private Dictionary<NetMsgID,Dictionary<object, ArrayList>> recCallBacks = new Dictionary<NetMsgID,Dictionary<object, ArrayList>>();
        
        //每次发送消息前都要保存下发的消息，用来处理断线重连或者发送超时后的补发情况
        //补发根据填充的数据顺序进行补发
        //private List<SendMsgCacheQueueData> logic_sendCache = new List<SendMsgCacheQueueData>();


    }
}
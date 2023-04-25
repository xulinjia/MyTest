
using GreyFramework;
using System;
using System.Collections;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using CenterMsg;
using Google.Protobuf;
using System.Reflection;

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
                //Managers.GetNetManager().reciveMsgs.Enqueue(message);
                Managers.GetNetManager().MessageDispatch(messageData.packetId, (uint)messageData.msgNum_server, (uint)messageData.msgNum_client, messageData.data);
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
            messageData.Reset();
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



        }

        protected override void OnUpdate()
        {
            //lock (reciveMsgs)
            //{
            //    int count = reciveMsgs.Count;
            //    for (int i = 0; i < count; i++)
            //    {
            //        MessageData message = reciveMsgs.Dequeue();//队列第一个添加的数据
            //        messageDispatch(message.packetId, (uint)message.msgNum_server, (uint)message.msgNum_client, message.data);
            //    }
            //}
        }
        public async Task TryConnectAsync(string ip, int post)
        {
            try
            {
                serverUri = new Uri("ws://localhost:4649/Chat");
                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(serverUri, CancellationToken.None);
                if (webSocket.State == WebSocketState.Open)
                {
                    Debug.LogError("连接成功！");
                    await ReceiveMessage();
                    startHeartBeat();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }


        //注册消息
        public void RegCallbackFunc(int msgId, object obj, Action<short, uint, uint, byte[]> action)
        {


        }

        //注销消息
        public void UnRegAllCallbackFunc(object obj)
        {

        }
        //发送消息
        public async Task SendMessageAsync(NetMsgID netMsgId, IMessage msgData)
        {

            short short_msgId = (short)netMsgId;
            byte[] byte_msgData = Serialize(msgData);
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
            await sendMessageAsync(sendBuffer.ToArray());
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
                        //sendMessage();
                        Debug.LogError("发送心跳包！");
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
        public static byte[] Serialize<T>(T obj) where T : IMessage
        {
            return obj.ToByteArray();
        }

        public static T Deserialize<T>(byte[] data) where T : class, IMessage, new()
        {
            T obj = new T();
            IMessage message = obj.Descriptor.Parser.ParseFrom(data);
            return message as T;
        }

        private async Task sendMessageAsync(byte[] message)
        {

            Debug.LogError("尝试发送消息:" + Encoding.UTF8.GetString(message));
            if (webSocket.State != WebSocketState.Open)
            {
                //await webSocket.ConnectAsync(serverUri, CancellationToken.None);
            }
            else if ((webSocket.State == WebSocketState.Open))
            {
               await webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }

        //消息处理
        //消息ID，服务器序列号，客户端序列号
        public void MessageDispatch(NetMsgID msgId, uint serverId, uint clientId, byte[] msgData)
        {
            Debug.LogError(msgId);
            Debug.LogError(Encoding.UTF8.GetString(msgData));
        }
        //public async Task ReceiveMessage()
        //{
        //    while (webSocket.State == WebSocketState.Open)
        //    {
        //        byte[] buffer = new byte[1024];
        //        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //        if (result.MessageType == WebSocketMessageType.Close)
        //        {
        //            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        //        }
        //        else
        //        {
        //            var str = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        //            Debug.LogError(str);
        //        }
        //    }
        //}

        private async Task ReceiveMessage()
        {
            while (true)
            {
                Debug.LogError("ReceiveMessage");
                if (webSocket.State != WebSocketState.Open)
                {
                    return;
                }
                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);
                await webSocket.ReceiveAsync(bytesReceived, CancellationToken.None);
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
                else
                {
                    Debug.LogError("网络已断开");
                }
            }
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
        private bool isStopReadBuffer = false;

        //每次发送消息前都要保存下发的消息，用来处理断线重连或者发送超时后的补发情况
        //补发根据填充的数据顺序进行补发
        //private List<SendMsgCacheQueueData> logic_sendCache = new List<SendMsgCacheQueueData>();


    }
}
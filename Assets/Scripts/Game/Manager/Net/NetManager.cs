
using GreyFramework;
using System;
using System.Collections;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ErisGame
{
    public class NetManager : Manager
    {
        public override eManager Type => eManager.Net;

        public override eHRESULT OnInitialization() { return eHRESULT.Succeed; }

        public override IEnumerator OnCreate()
        {
            return base.OnCreate();
        }
        protected override void OnUpdate()
        {
        
        }
        public async void TryConnect(string ip,int post) 
        {
            try
            {
                serverUri = new Uri("ws://localhost:4649/Chat");
                webSocket = new ClientWebSocket();
                // add header
                //ws.Options.SetRequestHeader("X-Token", "eyJhbGciOiJIUzI1N");
                await webSocket.ConnectAsync(serverUri, CancellationToken.None);
                if (webSocket.State == WebSocketState.Open)
                {
                    Debug.LogError("连接成功！");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            await ReceiveMessage();
        }

        public async Task SendMessage(string msg)
        {
            Debug.LogError("尝试发送消息"+ msg);
            if (webSocket.State != WebSocketState.Open)
            {
                await webSocket.ConnectAsync(serverUri, CancellationToken.None);
            }
            else if ((webSocket.State == WebSocketState.Open))
            {
                Debug.LogError("发送消息"+ msg);
                await webSocket.SendAsync(new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(msg)), WebSocketMessageType.Binary, true, CancellationToken.None);
            }

        }
        public async Task ReceiveMessage()
        {
            while(webSocket.State == WebSocketState.Open)
            {
                byte[] buffer = new byte[1024];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    var str = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.LogError(str);
                    Debug.LogError(result);
                }
            }
        }

        private Uri serverUri;
        private static ClientWebSocket webSocket;
        private object lockReconnect = new object();
        private Coroutine _pingCor, _clientPing, _serverPing;
    }
}

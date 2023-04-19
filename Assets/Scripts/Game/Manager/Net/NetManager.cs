
using GreyFramework;
using System;
using System.Collections;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ErisGame
{
    class NetManager : Manager
    {
        public override eManager Type => eManager.Net;
        public override eHRESULT OnInitialization() { return eHRESULT.Succeed; }
        public override IEnumerator OnCreate()
        {
            SendWebSocketRequest();

            return null;
        }

        public async void SendWebSocketRequest()
        {
            try
            {
                ClientWebSocket ws = new ClientWebSocket();
                CancellationToken ct = new CancellationToken();
                // add header
                //ws.Options.SetRequestHeader("X-Token", "eyJhbGciOiJIUzI1N");
                Uri url = new Uri("ws://localhost:4649/Chat");
                await ws.ConnectAsync(url, ct);
                await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("hello")), WebSocketMessageType.Binary, true, ct);
                while (true)
                {
                    var result = new byte[1024];
                    await ws.ReceiveAsync(new ArraySegment<byte>(result), new CancellationToken());
                    var str = Encoding.UTF8.GetString(result, 0, result.Length);
                    Debug.Log(str);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
    }
}

using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;


namespace TestWebsocket
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Console.WriteLine("Listening for incoming requests...");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    ProcessWebSocketRequest(context);
                }
                else
                {
                    context.Response.Close();
                }
            }
        }

        static async void ProcessWebSocketRequest(HttpListenerContext context)
        {
            HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
            WebSocket webSocket = webSocketContext.WebSocket;

            byte[] buffer = new byte[1024];
            ArraySegment<byte> bufferSegment = new ArraySegment<byte>(buffer);

            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(bufferSegment, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    break;
                }

                await webSocket.SendAsync(bufferSegment, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
            }
        }
    }
}
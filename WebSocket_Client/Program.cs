using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using (ClientWebSocket webSocket = new ClientWebSocket())
        {
            Uri serverUri = new Uri("ws://localhost:8080/");

            try
            {
                await webSocket.ConnectAsync(serverUri, CancellationToken.None);
                Console.WriteLine("Connected to the server.");

                Task receivingTask = Task.Run(async () =>
                {
                    byte[] receiveBuffer = new byte[1024];
                    ArraySegment<byte> bufferSegment = new ArraySegment<byte>(receiveBuffer);

                    while (webSocket.State == WebSocketState.Open)
                    {
                        WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(bufferSegment, CancellationToken.None);
                        string message = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
                        Console.WriteLine("Received: " + message);
                    }
                });

                Console.WriteLine("Enter a message (or 'exit' to quit):");
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input.ToLower() == "exit")
                        break;

                    byte[] sendBuffer = Encoding.UTF8.GetBytes(input);
                    ArraySegment<byte> bufferSegment = new ArraySegment<byte>(sendBuffer);

                    await webSocket.SendAsync(bufferSegment, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
                }

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
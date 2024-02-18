using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string ipAddress = "127.0.0.1"; // Use "0.0.0.0" for all available interfaces
        int port = 8080;

        HttpListener listener = new HttpListener();
        listener.Prefixes.Add($"http://{ipAddress}:{port}/");
        listener.Start();
        Console.WriteLine($"Server started at http://{ipAddress}:{port}/");

        while (true)
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HandleRequest(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    static void HandleRequest(HttpListenerContext context)
    {
        string responseString = "Hello from the server!";
        byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);

        context.Response.ContentType = "text/plain";
        context.Response.ContentLength64 = responseBytes.Length;

        using (Stream output = context.Response.OutputStream)
        using (StreamWriter writer = new StreamWriter(output))
        {
            foreach (byte b in responseBytes)
            {
                writer.Write((char)b);
                writer.Flush();
                System.Threading.Thread.Sleep(100); // Delay 100 milliseconds (10 bytes per second)
            }
        }

        Console.WriteLine($"Handled request from {context.Request.RemoteEndPoint}");
    }
}

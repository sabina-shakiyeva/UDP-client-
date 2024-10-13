using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("server");

var server = new UdpClient(5000);
var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
int count = 0;

while (true)
{
    var bytes = server.Receive(ref remoteEndPoint);
    var msg = Encoding.UTF8.GetString(bytes);

    if (msg == "EXIT")
    {
        Console.WriteLine("Images sent");
        break;
    }

    if (msg.StartsWith("start"))
    {
        count++;
        string clientFolder = $@"C:\Users\shaki\source\repos\UDP(client)\UDP(server)\{remoteEndPoint.Address}";
        Directory.CreateDirectory(clientFolder);
        var destination = $@"{clientFolder}\image_{count}.jpg";
        using (var fs = new FileStream(destination, FileMode.Create, FileAccess.Write))
        {
            while (true)
            {
                bytes = server.Receive(ref remoteEndPoint);
                msg = Encoding.UTF8.GetString(bytes);

                if (msg == "end")
                {
                    Console.WriteLine($"Image {count} saved to {destination}");
                    break;
                }
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}

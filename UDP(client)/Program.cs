using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Client Side");

var client = new UdpClient();
var endPoint = new IPEndPoint(IPAddress.Loopback, 5000);

while (true)
{
    Console.WriteLine("Enter File path(or write exit for end):");
    string filePath = Console.ReadLine();

    if (filePath == "exit")
    {
        var msg = Encoding.UTF8.GetBytes("EXIT");
        client.Send(msg, msg.Length, endPoint);
        break;
    }


    var startMsg = Encoding.UTF8.GetBytes("start");
    client.Send(startMsg, startMsg.Length, endPoint);

    using (var readFs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
    {
        int len = 0;
        var buffer = new byte[1024];
        while ((len = readFs.Read(buffer, 0, buffer.Length)) > 0)
        {
            client.Send(buffer, len, endPoint);
        }
        Console.WriteLine("Photo sent.");
    }


    var endMsg = Encoding.UTF8.GetBytes("end");
    client.Send(endMsg, endMsg.Length, endPoint);
}

client.Close();

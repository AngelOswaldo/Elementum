using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net; // TcpClient TcpListener UDPClient
using System.Net.Sockets;
using System.Text;

namespace Echo_Server
{
    enum example
    {
        IPaddress,
        TCPclient,
        TCPserver,
        UDPclient,
        UDPserver
    }
    class Program
    {
        static example Example = example.IPaddress;
        /*static void Main(string[] args)
        {
            switch (Example)
            {
                case example.IPaddress:
                    MainIPaddress(args);
                    break;
                case example.TCPclient:
                    MainTCPclient(args);
                    break;
                case example.TCPserver:
                    MainTCPserver(args);
                    break;
                case example.UDPclient:
                    MainUDPclient(args);
                    break;
                case example.UDPserver:
                    MainUDPserver(args);
                    break;
            }

        }*/

        #region Main functions
        static void MainIPaddress(string[] args)
        {
            try
            {
                var localHostName = Dns.GetHostName();
                PrintHostInfo(localHostName);
                PrintHostInfo("www.google.com");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void MainTCPclient(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                throw new ArgumentException("Parameters: <Server> <Word> [<Port>]");
            }
            var server = args[0];
            var word = Encoding.ASCII.GetBytes(args[1]);
            var port = args.Length == 3 ? int.Parse(args[2]) : 7;

            //TcpClient client = null;
            //NetworkStream stream = null;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //client = new TcpClient(server, port);
                //stream = client.GetStream();
                socket.Connect(server, port);
                Console.Write("Connected to server... sending echos string");

                //stream.Write(word, 0, word.Length);
                socket.Send(word, 0, word.Length, SocketFlags.None);
                Console.WriteLine($"Sent {word.Length} bytes to server");

                int totalReceived = 0;
                int received = 0;

                while (totalReceived < word.Length)
                {
                    //received = stream.Read(word, totalReceived, word.Length - totalReceived);
                    received = socket.Receive(word, totalReceived, word.Length - totalReceived, SocketFlags.None);
                    if (received == 0)
                    {
                        Console.WriteLine("Connection closed prematurely");
                        break;
                    }
                    totalReceived += received;
                }
                Console.WriteLine($"Received {totalReceived} bytes from server: {Encoding.ASCII.GetString(word)}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //stream.Close();
                //client.Close();
                socket.Close();
            }
        }
        static void MainTCPserver(string[] args)
        {
            if (args.Length > 1) throw new ArgumentException("Parameters: [<Port>]");

            int port = args.Length == 1 ? int.Parse(args[0]) : 7;
            //TcpListener listener = null;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //listener = new TcpListener(IPAddress.Any, port);
                //listener.Start();
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(5);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            byte[] receiveBuffer = new byte[32];
            int received = 0;

            while (true)
            {
                //TcpClient client = null;
                //NetworkStream stream = null;
                Socket client = null;

                try
                {
                    //client = listener.AcceptTcpClient();
                    //stream = client.GetStream();
                    client = socket.Accept();

                    int totalBytes = 0;
                    //received = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                    received = client.Receive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None);
                    while (received > 0)
                    {
                        //stream.Write(receiveBuffer, 0, received);
                        client.Send(receiveBuffer, 0, received, SocketFlags.None);
                        totalBytes += received;
                    }
                    Console.WriteLine($"Echoed {totalBytes} bytes");

                    //stream.Close();
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //stream.Close();
                }
            }
        }
        static void MainUDPclient(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                throw new System.ArgumentException("Parameters: <Server> <Word> [<Port>]");
            }

            var server = args[0];
            var word = Encoding.ASCII.GetBytes(args[1]);
            var port = args.Length == 3 ? int.Parse(args[2]) : 7;

            var client = new UdpClient();

            try
            {
                client.Send(word, word.Length, server, port);
                Console.WriteLine($"Sent {word.Length} bytes to the server...");

                var remote = new IPEndPoint(IPAddress.Any, 0);
                var received = client.Receive(ref remote);
                Console.WriteLine($"Received {received.Length} bytes from {remote}: {Encoding.ASCII.GetString(received, 0, received.Length)}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            client.Close();
        }
        static void MainUDPserver(string[] args)
        {
            if (args.Length > 1) throw new ArgumentException("Parameters: [<Port>]");

            var port = args.Length == 1 ? int.Parse(args[0]) : 7;

            UdpClient client = null;

            try
            {
                client = new UdpClient(port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var remote = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    var received = client.Receive(ref remote);
                    Console.WriteLine($"Handling client at: {remote}");

                    client.Send(received, received.Length, remote);
                    Console.Write($"Echoed {received.Length} bytes");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        #endregion

        static void PrintHostInfo(string host)
        {
            try
            {
                IPHostEntry hostInfo = Dns.GetHostEntry(host);
                Console.WriteLine($"\t{hostInfo.HostName}: ");
                Console.WriteLine("\tDirecciones: ");
                foreach (IPAddress adress in hostInfo.AddressList)
                {
                    Console.WriteLine("\t\t" + adress.ToString());
                }
                Console.WriteLine("\t\t Aliases: ");
                foreach (string alias in hostInfo.Aliases)
                {
                    Console.WriteLine("\t\t" + alias);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using EFlogger.Profiling.Network.Utils;

namespace EFlogger.Network.Network
{

    public class TCPServer
    {
        private readonly TcpListener _tcpListener;
        private Thread _listenThread;
        private bool _continueListen = true;

        public Action<byte[], TcpClient> MessageAccepted;

        public TCPServer()
        {

            _tcpListener = new TcpListener(IPAddress.Any, CommandSender.Port);
        }

        public void Start()
        {
            _listenThread = new Thread(ListenForClients);
            _listenThread.Start();
        }

        private void ListenForClients()
        {

            _tcpListener.Start();

            while (_continueListen)
            {
                //blocks until a client has connected to the server
                TcpClient client = _tcpListener.AcceptTcpClient();

                var clientThread = new Thread(HandleClientComm);
                clientThread.Start(client);
            }
            _tcpListener.Stop();


        }

        private void HandleClientComm(object client)
        {
            var tcpClient = (TcpClient)client;
            //tcpClient.ReceiveTimeout = 2;
            NetworkStream clientStream = tcpClient.GetStream();


            var ms = new MemoryStream();
            var binaryWriter = new BinaryWriter(ms);

            var message = new byte[tcpClient.ReceiveBufferSize];
            var message2 = new byte[4];
            int readCount;
            int totalReadMessageBytes = 0;

            clientStream.Read(message2, 0, 4);
            int messageLength = CommandUtils.BytesToInt(message2);

            while ((readCount = clientStream.Read(message, 0, tcpClient.ReceiveBufferSize)) != 0)
            {
                binaryWriter.Write(message, 0, readCount);
                totalReadMessageBytes += readCount;
                if (totalReadMessageBytes >= messageLength)
                    break;
            }

            if (ms.Length > 0)
            {
                MessageAccepted(ms.ToArray(), tcpClient);
            }
        }

        public void Stop()
        {
            _continueListen = false;
            _tcpListener.Stop();
            _listenThread.Abort();


        }
    }
}

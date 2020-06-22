using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using EFlogger.Network.Commands;
using EFlogger.Network.Enums;

namespace EFlogger.Network.Network
{
    public class CommandListener
    {
        private TCPServer _tcpServer;

        public Action<QueryCommand, TcpClient> OnQueryCommand;
        public Action<TcpClient> OnClearLogDataGrid;


        public void Start()
        {
            _tcpServer = new TCPServer();
            _tcpServer.MessageAccepted += MessageAccepted;
            _tcpServer.Start();
        }


        private void MessageAccepted(byte[] bytes, TcpClient tcpClient)
        {
            Parse(bytes, tcpClient);
        }

        private void Parse(byte[] bytes, TcpClient tcpClient)
        {
            if (bytes.Length >= CommandHeader.GetLenght())
            {
                CommandHeader commandHeader = CommandHeader.FromBytes(bytes);
                IEnumerable<byte> nextCommandBytes = bytes.Skip(CommandHeader.GetLenght());
                switch ((CommandTypeEnum)commandHeader.Type)
                {
                    case CommandTypeEnum.QueryCommand:
                        QueryCommand presentationFileCommand = QueryCommand.FromBytes(nextCommandBytes.ToArray());
                        OnQueryCommand(presentationFileCommand, tcpClient);
                        break;
                    case CommandTypeEnum.ClearLogDataGrid:
                        OnClearLogDataGrid(tcpClient);
                        break;
                }
            }
        }

        public void Stop()
        {
            _tcpServer.Stop();
        }
    }
}

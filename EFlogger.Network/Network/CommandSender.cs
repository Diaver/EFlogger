using System.Diagnostics;
using System.Net.Sockets;
using EFlogger.Network.Commands;
using EFlogger.Network.Enums;
using EFlogger.Profiling.Network.Utils;

namespace EFlogger.Network.Network
{
    public static class CommandSender
    {
        public const int Port = 45823;
        public static string IP = "127.0.0.1";
        

        public static void SendMessageAccepted()
        {
            var commandHeader = new CommandHeader{
                Count = 1, 
                Type = (int) CommandTypeEnum.MessageAccepted
            };
            SendAnswer(IP,Port, commandHeader.ToBytes());
        }


        public static void SendClearLogDataGrid()
        {
            var commandHeader = new CommandHeader
            {
                Count = 1,
                Type = (int)CommandTypeEnum.ClearLogDataGrid
            };
            SendAnswer(IP, Port, commandHeader.ToBytes());
        }

        public static void SendQueryCommand(QueryCommand queryCommand)
        {
            var commandHeader = new CommandHeader
            {
                Count = 1,
                Type = (int)CommandTypeEnum.QueryCommand
            };

            
            byte[] commandBytes = CommandUtils.ConcatByteArrays(commandHeader.ToBytes(), queryCommand.ToBytes());
            SendAnswer(IP, Port, commandBytes);
        }


        private static void SendAnswer(string ipAddress, int port, byte[] messageBytes)
        {
            var client = new TcpClient();

            try
            {
                client.Connect(ipAddress, port);

                byte[] messageBytesWithEof = CommandUtils.AddCommandLength(messageBytes);
                NetworkStream networkStream = client.GetStream();
                networkStream.Write(messageBytesWithEof, 0, messageBytesWithEof.Length);
                networkStream.Close();
                client.Close();
            }
            catch (SocketException exception) //hide exception,
            {
                Trace.WriteLine(exception.Message + " " + exception.InnerException);
               
            }
            
            
          
        }
    }
}

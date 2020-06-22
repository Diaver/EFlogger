using System.IO;
using EFlogger.Profiling.Network.Utils;

namespace EFlogger.Network.Commands
{
    public class QueryCommand
    {
        private int CommandTextLenght { get; set; }
        public string CommandText { get; set; }

        public string CommandTextOriginal { get; set; }

        public int ResultRowsCount { get; set; }
        public long QueryMiliseconds { get; set; }

        private int CreatedLenght { get; set; }
        public string Created { get; set; }

        private int MethodNameLenght { get; set; }
        public string MethodName { get; set; }

        private int ClassNameLenght { get; set; }
        public string ClassName { get; set; }

        private int MethodBodyLenght { get; set; }
        public string MethodBody { get; set; }


        private int StackTraceLenght { get; set; }
        public string StackTrace { get; set; }

        public byte[] ToBytes()
        {
            byte[] commandTextBytes = CommandUtils.GetBytes(CommandText);
            byte[] createdBytes = CommandUtils.GetBytes(Created);
            byte[] methodNameBytes = CommandUtils.GetBytes(MethodName);
            byte[] classNameBytes = CommandUtils.GetBytes(ClassName);
            byte[] methodBodyBytes = CommandUtils.GetBytes(MethodBody);
            byte[] stackTraceBytes = CommandUtils.GetBytes(StackTrace);

            CommandTextLenght = commandTextBytes.Length;
            CreatedLenght = createdBytes.Length;
            MethodNameLenght = methodNameBytes.Length;
            ClassNameLenght = classNameBytes.Length;
            MethodBodyLenght = methodBodyBytes.Length;
            StackTraceLenght = stackTraceBytes.Length;

            int messageLenght = sizeof(int) * 7 + sizeof(long) + CommandTextLenght + CreatedLenght + MethodNameLenght + ClassNameLenght + MethodBodyLenght + StackTraceLenght;

            var messageData = new byte[messageLenght];
            using (var stream = new MemoryStream(messageData))
            {
                var writer = new BinaryWriter(stream);
                writer.Write(CommandTextLenght);
                writer.Write(commandTextBytes);

                writer.Write(ResultRowsCount);
                writer.Write(QueryMiliseconds);

                writer.Write(CreatedLenght);
                writer.Write(createdBytes);

                writer.Write(MethodNameLenght);
                writer.Write(methodNameBytes);

                writer.Write(ClassNameLenght);
                writer.Write(classNameBytes);

                writer.Write(MethodBodyLenght);
                writer.Write(methodBodyBytes);

               

                writer.Write(StackTraceLenght);
                writer.Write(stackTraceBytes);

                return messageData;
            }

        }

        public static QueryCommand FromBytes(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var br = new BinaryReader(ms);
                var command = new QueryCommand();

                command.CommandTextLenght = br.ReadInt32();
                command.CommandText = CommandUtils.GetString(br.ReadBytes(command.CommandTextLenght));

                command.ResultRowsCount = br.ReadInt32();
                command.QueryMiliseconds = br.ReadInt64();

                command.CreatedLenght = br.ReadInt32();
                command.Created = CommandUtils.GetString(br.ReadBytes(command.CreatedLenght));

                command.MethodNameLenght = br.ReadInt32();
                command.MethodName = CommandUtils.GetString(br.ReadBytes(command.MethodNameLenght));

                command.ClassNameLenght = br.ReadInt32();
                command.ClassName = CommandUtils.GetString(br.ReadBytes(command.ClassNameLenght));

                command.MethodBodyLenght = br.ReadInt32();
                command.MethodBody = CommandUtils.GetString(br.ReadBytes(command.MethodBodyLenght));
             
                command.StackTraceLenght = br.ReadInt32();
                command.StackTrace = CommandUtils.GetString(br.ReadBytes(command.StackTraceLenght));

                return command;
            }
        }
    }
}

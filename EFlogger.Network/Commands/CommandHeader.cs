using System.IO;

namespace EFlogger.Network.Commands
{
    public struct CommandHeader
    {
        public int Type { get; set; }
        public int Count { get; set; }

        public static int GetLenght()
        {
            return sizeof(int) + sizeof(int);
        }

        public static  CommandHeader FromBytes(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var br = new BinaryReader(ms);
                var currentObject = new CommandHeader();

                currentObject.Type = br.ReadInt32();
                currentObject.Count = br.ReadInt32();

                return currentObject;
            }
        }

        public  byte[] ToBytes()
        {
            byte[] data = new byte[GetLenght()];
            using (var stream = new MemoryStream(data))
            {
                var writer = new BinaryWriter(stream);
                writer.Write(Type);
                writer.Write(Count);
                return data;
            }

        }
    }
}

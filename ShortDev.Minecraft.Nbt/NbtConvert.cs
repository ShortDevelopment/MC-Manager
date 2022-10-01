using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ShortDev.Minecraft.Nbt
{
    public static class NbtConvert
    {
        public static NbtTag? Deserialize(string filePath, ref NbtConvertOptions options)
        {
            using FileStream fileStream = File.OpenRead(filePath);

            switch (options.Compression)
            {
                case NbtCompression.Gzip:
                    using (GZipStream decompressedStream = new(fileStream, CompressionMode.Decompress))
                    using (MemoryStream outStream = new())
                    {
                        decompressedStream.CopyTo(outStream);
                        outStream.Position = 0;
                        return Deserialize(outStream, ref options);
                    }
                case NbtCompression.None:
                    return Deserialize(fileStream, ref options);
            }
            throw new NotImplementedException();
        }

        public static NbtTag? Deserialize(Stream stream, ref NbtConvertOptions options)
        {
            using BinaryReader reader = options.UseBigEndian ? new BigEndianBinaryReader(stream) : new BinaryReader(stream);

            if (options.UseLevelHeader)
            {
                options.FileVersion = reader.ReadInt32();
                int bodyLength = reader.ReadInt32();
            }

            return ParseTag(reader);
        }

        internal static NbtTag? ParseTag(BinaryReader reader)
        {
            var type = (NbtTagType)reader.ReadByte();
            if (type == NbtTagType.End)
                return null;

            string? name = ReadString(reader);
            var tag = ParseTagValue(reader, type);
            if (tag != null)
                tag.Name = name;
            return tag;
        }

        internal static NbtTag? ParseTagValue(BinaryReader reader, NbtTagType type)
        {
            switch (type)
            {
                case NbtTagType.Byte:
                case NbtTagType.Short:
                case NbtTagType.Int:
                case NbtTagType.Long:
                case NbtTagType.Float:
                case NbtTagType.Double:
                case NbtTagType.String:
                    {
                        Tags.NbtValue tag = new();
                        tag.Type = type;
                        tag.PopulateValue(reader);
                        return tag;
                    }
                case NbtTagType.ByteArray:
                case NbtTagType.IntArray:
                case NbtTagType.LongArray:
                case NbtTagType.List:
                    {
                        Tags.NbtCollection tag = new();
                        tag.Type = type;
                        tag.PopulateValue(reader);
                        return tag;
                    }
                case NbtTagType.Compound:
                    {
                        Tags.NbtCompound tag = new();
                        tag.Type = type;
                        tag.PopulateValue(reader);
                        return tag;
                    }
                case NbtTagType.End:
                    return null;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void Serialize(string fileName, NbtTag tag, NbtConvertOptions options)
        {
            using (var fileStream = File.Create(fileName))
                Serialize(fileStream, tag, options);
        }

        public static void Serialize(Stream stream, NbtTag tag, NbtConvertOptions options)
        {
            if (options.Compression != NbtCompression.None || options.UseBigEndian)
                throw new ArgumentException();

            using (BinaryWriter writer = new(stream))
            {
                if (options.UseLevelHeader)
                {
                    writer.Write(options.FileVersion);
                    writer.Write((int)0); // ToDo: Length
                }

                WriteTag(writer, tag);
            }
        }

        internal static void WriteTag(BinaryWriter writer, NbtTag tag)
        {
            writer.Write((byte)tag.Type);
            WriteString(writer, tag.Name);
            tag.WriteValue(writer);
        }

        internal static string? ReadString(BinaryReader reader)
        {
            ushort length = reader.ReadUInt16();
            if (length == 0)
                return null;
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }

        internal static void WriteString(BinaryWriter writer, string? value)
        {
            if (value == null)
                writer.Write((short)0);
            else
            {
                writer.Write((short)value.Length);
                writer.Write(Encoding.UTF8.GetBytes(value));
            }
        }
    }

    public sealed class NbtConvertOptions
    {
        public NbtCompression Compression { get; init; }
        public bool UseBigEndian { get; init; }
        public bool UseLevelHeader { get; init; }

        public int FileVersion { get; set; }

        public static NbtConvertOptions BedrockLevel
            => new()
            {
                Compression = NbtCompression.None,
                UseBigEndian = false,
                UseLevelHeader = true
            };

        public static NbtConvertOptions BedrockNbtStructure
            => new()
            {
                Compression = NbtCompression.Gzip,
                UseBigEndian = true
            };

        public static NbtConvertOptions McStruct
            => new()
            {
                Compression = NbtCompression.None,
                UseBigEndian = false
            };
    }

    public enum NbtCompression
    {
        None,
        Gzip
    }

    class BigEndianBinaryReader : BinaryReader
    {
        public BigEndianBinaryReader(Stream stream) : base(stream) { }

        public override Int16 ReadInt16()
        {
            var data = base.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }
        public override UInt16 ReadUInt16()
        {
            var data = base.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }

        public override int ReadInt32()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }
        public override UInt32 ReadUInt32()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }

        public override Int64 ReadInt64()
        {
            var data = base.ReadBytes(8);
            Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }
        public override UInt64 ReadUInt64()
        {
            var data = base.ReadBytes(8);
            Array.Reverse(data);
            return BitConverter.ToUInt64(data, 0);
        }
    }
}

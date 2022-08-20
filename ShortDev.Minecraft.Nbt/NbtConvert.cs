using System;
using System.IO;
using System.IO.Compression;

namespace ShortDev.Minecraft.Nbt
{
    public static class NbtConvert
    {
        public static NbtTag? Convert(string filePath, NbtConvertOptions options)
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
                        return Convert(outStream, options);
                    }
                case NbtCompression.None:
                    return Convert(fileStream, options);
            }
            throw new NotImplementedException();
        }

        public static NbtTag? Convert(Stream stream, NbtConvertOptions options)
        {
            using BinaryReader reader = options.UseBigEndian ? new BigEndianBinaryReader(stream) : new BinaryReader(stream);

            if (options.UseLevelHeader)
            {
                options.FileVersion = reader.ReadInt32();
                int bodyLength = reader.ReadInt32();
            }

            TryParseInternal(null, reader, out var tag);
            return tag;
        }

        internal static bool TryParseInternal(NbtTag? parentTag, BinaryReader reader, out NbtTag? tag)
        {
            tag = null;

            if (reader.BaseStream.Position >= reader.BaseStream.Length - 1)
                return false;

            var type = (NbtTagType)reader.ReadByte();
            if (type == NbtTagType.End)
                return false;

            ushort nameLength = reader.ReadUInt16();
            byte[] nameData = reader.ReadBytes(nameLength);
            var name = System.Text.Encoding.UTF8.GetString(nameData);

            tag = ParseTag(type, reader);
            if (tag != null)
                tag.Name = name;

            return true;
        }

        internal static NbtTag? ParseTag(NbtTagType type, BinaryReader reader)
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
                        tag.Populate(reader);
                        return tag;
                    }
                case NbtTagType.ByteArray:
                case NbtTagType.IntArray:
                case NbtTagType.LongArray:
                case NbtTagType.List:
                    {
                        Tags.NbtCollection tag = new();
                        tag.Type = type;
                        tag.Populate(reader);
                        return tag;
                    }
                case NbtTagType.Compound:
                    {
                        Tags.NbtCompound tag = new();
                        tag.Type = type;
                        tag.Populate(reader);
                        return tag;
                    }
                case NbtTagType.End:
                    return null;
            }
            throw new NotImplementedException();
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

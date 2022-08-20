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

            NbtTag tag = new();
            while (ParseInternal(tag, reader, out _)) { }
            return tag;
        }

        // https://github.dev/natiiix/NbtEditor
        static bool ParseInternal(NbtTag? parentTag, BinaryReader reader, out NbtTag tag)
        {
            tag = new();

            if (reader.BaseStream.Position >= reader.BaseStream.Length - 1)
                return false;

            tag.Type = (NbtTagType)reader.ReadByte();
            if (tag.Type == NbtTagType.End)
                return false;

            //if (reader.BaseStream.Position >= reader.BaseStream.Length - 10)
            //    return false;

            ushort nameLength = reader.ReadUInt16();
            byte[] nameData = reader.ReadBytes(nameLength);
            tag.Name = System.Text.Encoding.UTF8.GetString(nameData);

            if (tag.Type == NbtTagType.Compound)
                while (ParseInternal(tag, reader, out _)) { }
            else
                tag.Value = ParseTagValue(tag.Type, reader);

            parentTag?.Children.Add(tag);
            return true;
        }

        static object? ParseTagValue(NbtTagType type, BinaryReader reader)
        {
            switch (type)
            {
                case NbtTagType.Byte:
                    return reader.ReadByte();
                case NbtTagType.Short:
                    return reader.ReadInt16();
                case NbtTagType.Int:
                    return reader.ReadInt32();
                case NbtTagType.Long:
                    return reader.ReadInt64();
                case NbtTagType.Float:
                    return reader.ReadSingle();

                case NbtTagType.Double:
                    return reader.ReadDouble();
                case NbtTagType.String:
                    {
                        ushort length = reader.ReadUInt16();
                        return System.Text.Encoding.UTF8.GetString(reader.ReadBytes(length));
                    }
                case NbtTagType.ByteArray:
                    {
                        int length = reader.ReadInt32();
                        return reader.ReadBytes(length);

                    }
                case NbtTagType.IntArray:
                    {
                        int length = reader.ReadInt32();
                        int[] buffer = new int[length];
                        for (int i = 0; i < length; i++)
                            buffer[i] = reader.ReadInt32();
                        return buffer;
                    }
                case NbtTagType.LongArray:
                    {
                        int length = reader.ReadInt32();
                        long[] buffer = new long[length];
                        for (int i = 0; i < length; i++)
                            buffer[i] = reader.ReadInt64();
                        return buffer;
                    }
                case NbtTagType.List:
                    {
                        NbtTagType listType = (NbtTagType)reader.ReadByte();
                        int length = reader.ReadInt32();
                        NbtTag[] buffer = new NbtTag[length];
                        for (int i = 0; i < length; i++)
                            buffer[i] = new()
                            {
                                Type = listType,
                                Value = ParseTagValue(listType, reader)
                            };
                        return buffer;
                    }
                case NbtTagType.Compound:
                    {
                        NbtTag tag = new();
                        tag.Type = NbtTagType.Compound;
                        while (ParseInternal(tag, reader, out _)) { }
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

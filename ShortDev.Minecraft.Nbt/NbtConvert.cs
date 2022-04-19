using System;
using System.IO;
using System.IO.Compression;

namespace ShortDev.Minecraft.Nbt
{
    public static class NbtConvert
    {
        public static NbtTag? Convert(string filePath, bool useGzip = true)
        {
            using FileStream fileStream = File.OpenRead(filePath);
            using MemoryStream outStream = new();

            if (useGzip)
                using (var decompressedStream = new GZipStream(fileStream, CompressionMode.Decompress))
                    return Convert(decompressedStream);
            else
                return Convert(fileStream);
        }

        public static NbtTag? Convert(Stream stream)
        {
            using BinaryReader reader = new(stream);
            NbtTag tag = new();
            ParseInternal(null, ref tag, reader);
            return tag;
        }

        // https://github.dev/natiiix/NbtEditor
        public static void ParseInternal(NbtTag? parent, ref NbtTag hostTag, BinaryReader reader)
        {
            NbtTag tag = new();
            tag.Type = (NbtTagType)reader.ReadByte();
            if (tag.Type == NbtTagType.End)
            {
                if (parent == null)
                    return;
                hostTag = parent;
            }

            if (reader.BaseStream.Position >= reader.BaseStream.Length - 10)
                return;

            ushort nameLength = reader.ReadUInt16();
            byte[] nameData = reader.ReadBytes(nameLength);
            tag.Name = System.Text.Encoding.UTF8.GetString(nameData);



            hostTag.Children.Add(tag);
            ParseInternal(tag, ref tag, reader);
        }
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

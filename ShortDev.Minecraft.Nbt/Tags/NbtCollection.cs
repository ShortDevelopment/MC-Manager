using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShortDev.Minecraft.Nbt.Tags
{
    [JsonConverter(typeof(NbtJsonConverter))]
    public sealed class NbtCollection : NbtTag
    {
        public Array? Items { get; set; }

        public IEnumerable<T> Enumerate<T>()
            => (IEnumerable<T>)Items;

        public override T GetItem<T>(int index)
             => (T)Items.GetValue(index);

        internal override void PopulateValue(BinaryReader reader)
        {
            switch (Type)
            {
                case NbtTagType.ByteArray:
                    {
                        int length = reader.ReadInt32();
                        Items = reader.ReadBytes(length);
                        break;
                    }
                case NbtTagType.IntArray:
                    {
                        int length = reader.ReadInt32();
                        int[] buffer = new int[length];
                        for (int i = 0; i < length; i++)
                            buffer[i] = reader.ReadInt32();
                        Items = buffer;
                        break;
                    }
                case NbtTagType.LongArray:
                    {
                        int length = reader.ReadInt32();
                        long[] buffer = new long[length];
                        for (int i = 0; i < length; i++)
                            buffer[i] = reader.ReadInt64();
                        Items = buffer;
                        break;
                    }
                case NbtTagType.List:
                    {
                        NbtTagType listType = (NbtTagType)reader.ReadByte();
                        int length = reader.ReadInt32();
                        NbtTag?[] buffer = new NbtTag[length];
                        for (int i = 0; i < length; i++)
                            buffer[i] = NbtConvert.ParseTagValue(reader, listType);
                        Items = buffer;
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        internal override void WriteValue(BinaryWriter writer)
        {
            if (Items == null)
                throw new InvalidDataException();

            switch (Type)
            {
                case NbtTagType.ByteArray:
                    {
                        writer.Write((int)Items.Length);
                        writer.Write((byte[])Items);
                        break;
                    }
                case NbtTagType.IntArray:
                    {
                        writer.Write((int)Items.Length);
                        foreach (var item in (int[])Items)
                            writer.Write(item);
                        break;
                    }
                case NbtTagType.LongArray:
                    {
                        writer.Write((int)Items.Length);
                        foreach (var item in (long[])Items)
                            writer.Write(item);
                        break;
                    }
                case NbtTagType.List:
                    {
                        NbtTagType listType = NbtTagType.End;
                        if (Items.Length > 0)
                            listType = ((NbtTag)Items.GetValue(0)!).Type;
                        writer.Write((byte)listType);
                        writer.Write((int)Items.Length);
                        foreach (var tag in (NbtTag[])Items)
                            tag.WriteValue(writer);
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public sealed class NbtJsonConverter : JsonConverter<NbtCollection>
        {
            public override NbtCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, NbtCollection value, JsonSerializerOptions options)
            {
                writer.WriteStartArray(value.Name ?? "");
                foreach (var item in value.Items)
                    JsonSerializer.Serialize(writer, item, options);
                writer.WriteEndArray();
            }
        }
    }
}

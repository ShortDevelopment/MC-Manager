using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShortDev.Minecraft.Nbt.Tags
{
    [JsonConverter(typeof(NbtJsonConverter))]
    public sealed class NbtCollection : NbtTag
    {
        public IEnumerable Items { get; set; }

        internal override void Populate(BinaryReader reader)
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
                            buffer[i] = NbtConvert.ParseTag(listType, reader);
                        Items = buffer;
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

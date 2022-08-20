using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShortDev.Minecraft.Nbt.Tags
{
    [JsonConverter(typeof(NbtJsonConverter))]
    public sealed class NbtValue : NbtTag
    {
        public object? Value { get; set; }

        internal override void Populate(BinaryReader reader)
        {
            switch (Type)
            {
                case NbtTagType.Byte:
                    Value = reader.ReadByte();
                    break;
                case NbtTagType.Short:
                    Value = reader.ReadInt16();
                    break;
                case NbtTagType.Int:
                    Value = reader.ReadInt32();
                    break;
                case NbtTagType.Long:
                    Value = reader.ReadInt64();
                    break;
                case NbtTagType.Float:
                    Value = reader.ReadSingle();
                    break;
                case NbtTagType.Double:
                    Value = reader.ReadDouble();
                    break;
                case NbtTagType.String:
                    {
                        ushort length = reader.ReadUInt16();
                        Value = Encoding.UTF8.GetString(reader.ReadBytes(length));
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public sealed class NbtJsonConverter : JsonConverter<NbtValue>
        {
            public override NbtValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, NbtValue value, JsonSerializerOptions options)
            {
                string name = value.Name ?? "";
                switch (value.Value)
                {
                    case sbyte:
                    case short:
                    case int:
                    case long:
                        writer.WriteNumber(name, (long)value.Value);
                        break;
                    case byte:
                    case ushort:
                    case uint:
                    case ulong:
                        writer.WriteNumber(name, (ulong)value.Value);
                        break;
                    case float:
                    case double:
                    case decimal:
                        writer.WriteNumber(name, (decimal)value.Value);
                        break;
                    case string:
                        writer.WriteString(name, (string)value.Value);
                        break;
                    default:
                        JsonSerializer.Serialize(writer, value.Value, options);
                        break;
                }
            }
        }
    }
}

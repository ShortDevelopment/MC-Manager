using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShortDev.Minecraft.Nbt.Tags
{
    [JsonConverter(typeof(NbtJsonConverter))]
    public sealed class NbtValue : NbtTag
    {
        public object? Value { get; set; }

        internal override void PopulateValue(BinaryReader reader)
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
                        Value = NbtConvert.ReadString(reader);
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        internal override void WriteValue(BinaryWriter writer)
        {
            if (Type != NbtTagType.String && Value == null)
                throw new InvalidDataException();

            switch (Type)
            {
                case NbtTagType.Byte:
                    writer.Write((byte)Value);
                    break;
                case NbtTagType.Short:
                    writer.Write((short)Value);
                    break;
                case NbtTagType.Int:
                    writer.Write((int)Value);
                    break;
                case NbtTagType.Long:
                    writer.Write((long)Value);
                    break;
                case NbtTagType.Float:
                    writer.Write((float)Value);
                    break;
                case NbtTagType.Double:
                    writer.Write((double)Value);
                    break;
                case NbtTagType.String:
                    {
                        NbtConvert.WriteString(writer, (string)Value);
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

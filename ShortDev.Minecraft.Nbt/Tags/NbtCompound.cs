using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShortDev.Minecraft.Nbt.Tags
{
    [JsonConverter(typeof(NbtJsonConverter))]
    public sealed class NbtCompound : NbtTag
    {
        public List<NbtTag?> Children { get; set; } = new();

        public override T GetItem<T>(int index)
             => (T)Children[index];

        public override T GetItem<T>(string name)
           => (T)Children.FirstOrDefault((x) => x?.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);

        public string?[] Keys
            => Children.Select((x) => x.Name).ToArray();

        internal override void PopulateValue(BinaryReader reader)
        {
            if (Type != NbtTagType.Compound)
                throw new NotImplementedException();

            while (true)
            {
                var tag = NbtConvert.ParseTag(reader);
                if (tag == null)
                    break;
                Children.Add(tag);
            }
        }

        internal override void WriteValue(BinaryWriter writer)
        {
            if (Type != NbtTagType.Compound)
                throw new NotImplementedException();

            foreach (var tag in Children)
                NbtConvert.WriteTag(writer, tag!);
            writer.Write((byte)NbtTagType.End);
        }

        public sealed class NbtJsonConverter : JsonConverter<NbtCompound>
        {
            public override NbtCompound? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, NbtCompound value, JsonSerializerOptions options)
            {
                writer.WriteStartObject(value.Name ?? "");
                foreach (var child in value.Children)
                    JsonSerializer.Serialize(writer, child, options);
                writer.WriteEndObject();
            }
        }
    }
}
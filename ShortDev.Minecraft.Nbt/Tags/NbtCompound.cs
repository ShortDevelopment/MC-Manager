using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShortDev.Minecraft.Nbt.Tags
{
    [JsonConverter(typeof(NbtJsonConverter))]
    public sealed class NbtCompound : NbtTag
    {
        public List<NbtTag?> Children { get; set; } = new();

        internal override void Populate(BinaryReader reader)
        {
            if (Type != NbtTagType.Compound)
                throw new NotImplementedException();

            while (NbtConvert.TryParseInternal(null, reader, out var tag))
                Children.Add(tag);
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
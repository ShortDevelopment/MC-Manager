
using System.IO;
using System.Text.Json;

namespace ShortDev.Minecraft.Nbt
{
    public abstract class NbtTag
    {
        public NbtTagType Type { get; set; }

        public string? Name { get; set; }

        internal abstract void PopulateValue(BinaryReader reader);
        internal abstract void WriteValue(BinaryWriter writer);

        public virtual T? GetItem<T>(int index) where T : NbtTag
            => null;

        public virtual T? GetItem<T>(string name) where T : NbtTag
            => null;

        public override string ToString()
            => JsonSerializer.Serialize(this);
    }
}

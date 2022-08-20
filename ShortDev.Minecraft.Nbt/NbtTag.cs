
using System.IO;
using System.Text.Json;

namespace ShortDev.Minecraft.Nbt
{
    public abstract class NbtTag
    {
        public NbtTagType Type { get; set; }

        public string? Name { get; set; }

        internal abstract void Populate(BinaryReader reader);

        public override string ToString()
            => JsonSerializer.Serialize(this);
    }
}

using System.Collections.Generic;

namespace ShortDev.Minecraft.Nbt
{
    public class NbtTag
    {
        public NbtTagType Type { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }

        public List<NbtTag> Children { get; set; } = new();
    }
}

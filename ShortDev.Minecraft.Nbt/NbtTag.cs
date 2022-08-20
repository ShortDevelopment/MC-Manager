using System;
using System.Collections;
using System.Collections.Generic;

namespace ShortDev.Minecraft.Nbt
{
    public class NbtTag : IEnumerable<NbtTag>
    {
        public NbtTagType Type { get; set; }

        public string? Name { get; set; }

        public object? Value { get; set; }

        public List<NbtTag> Children { get; set; } = new();

        public override string ToString()
            => ToString(0);

        public string ToString(int nesting)
        {
            string result = $"{"".PadLeft(nesting)}[{Type}] \"{Name ?? "null"}\": ";

            if (Value == null)
                result += "null";
            else if (Value is NbtTag tag)
                result += Environment.NewLine + tag.ToString(nesting + 1);
            else if (Value is NbtTag[] children)
                foreach (var child in children)
                    result += Environment.NewLine + child.ToString(nesting + 1);
            else
                result += Value.ToString();

            foreach (var child in Children)
                result += Environment.NewLine + child.ToString(nesting + 1);

            return result;
        }

        public IEnumerator<NbtTag> GetEnumerator()
            => Children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}

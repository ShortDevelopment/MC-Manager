namespace ShortDev.Minecraft.Nbt
{
    /// <summary>
    /// <see href="https://minecraft.fandom.com/wiki/NBT_format#TAG_definition">TAG definition</see>
    /// </summary>
    public enum NbtTagType : byte
    {
        End,
        Byte,
        Short,
        Int,
        Long,
        Float,
        Double,
        ByteArray,
        String,
        List,
        Compound,
        IntArray,
        LongArray
    }
}

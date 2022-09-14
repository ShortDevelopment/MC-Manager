using ShortDev.Minecraft.Nbt;
using ShortDev.Minecraft.Nbt.Tags;
using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = NbtConvertOptions.BedrockLevel;
            var tag = (NbtCompound)NbtConvert.Deserialize(
                @"C:\Users\lukas\AppData\Local\Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang\minecraftWorlds\U9T6YO7zEgA=\level.dat",
                ref options
            );

            //var tag2 = NbtConvert.Convert(
            //    @"C:\Program Files\WindowsApps\Microsoft.MinecraftUWP_1.19.2002.0_x64__8wekyb3d8bbwe\data\structures\village\taiga\taiga_decoration_1.nbt",
            //    NbtConvertOptions.BedrockNbtStructure
            //);

            var names = tag.GetItem<NbtCompound>("experiments").Keys;

            tag = (NbtCompound)NbtConvert.Deserialize(
                @"...",
                ref options
            );

            var experiments = tag.GetItem<NbtCompound>("experiments");
            experiments.Children.Clear();

            foreach (var name in names)
            {
                experiments.Children.Add(new NbtValue()
                {
                    Type = NbtTagType.Byte,
                    Name = name,
                    Value = (byte)1
                });
            }

            NbtConvert.Serialize(@"...", tag, options);

            Console.ReadLine();
        }
    }
}

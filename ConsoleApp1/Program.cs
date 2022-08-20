using ShortDev.Minecraft.Nbt;
using System;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var tag = NbtConvert.Convert(
            //    @"C:\Users\lukas\AppData\Local\Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang\minecraftWorlds\9BTBYLt5AAA=\level.dat",
            //    NbtConvertOptions.BedrockLevel
            //);

            var tag2 = NbtConvert.Convert(
                @"C:\Program Files\WindowsApps\Microsoft.MinecraftUWP_1.19.2002.0_x64__8wekyb3d8bbwe\data\structures\village\taiga\taiga_decoration_1.nbt",
                NbtConvertOptions.BedrockNbtStructure
            );

            Console.WriteLine(tag2.ToString());

            Console.ReadLine();
        }
    }
}

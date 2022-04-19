using ShortDev.Minecraft.Nbt;
using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"C:\Users\lukas\AppData\Local\Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang\minecraftWorlds\9BTBYLt5AAA=";
            var tag = NbtConvert.Convert(Path.Combine(folderPath, "level.dat"), useGzip: false);
            //do
            //{
            //    Console.WriteLine($"Type: {tag.Type}, Name: {tag.Name}");
            //    tag = tag.Child;
            //} while (tag != null);

            Console.ReadLine();
        }
    }
}

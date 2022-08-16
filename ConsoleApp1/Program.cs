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

            Print(tag, 0);

            Console.ReadLine();
        }

        static void Print(NbtTag tag, int nesting)
        {
            Console.WriteLine($"{"".PadLeft(nesting)}{tag.Name ?? "null"}: {tag.Value ?? "null"}");

            foreach (var child in tag.Children)
                Print(child, nesting + 1);
        }
    }
}

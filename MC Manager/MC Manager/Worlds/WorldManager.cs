using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MC_Manager.Worlds
{
    public class WorldManager
    {
        public static string McWorldsPath { get => System.IO.Path.Combine(Utils.MinecraftPath, @"LocalState\games\com.mojang\minecraftWorlds\"); }

        public static async Task<WorldInfo[]> GetWorldsAsync()
        {
            StorageFolder worldsDir = await StorageFolder.GetFolderFromPathAsync(McWorldsPath);
            return (await worldsDir.GetFoldersAsync()).Select((folder) => new WorldInfo(folder)).ToArray();
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MC_Manager.Worlds
{
    public class WorldManager
    {
        public static string McWorldsPath { get => System.IO.Path.Combine(Utils.UserLocalAppData, @"Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang\minecraftWorlds\"); }

        public static async Task<WorldInfo[]> GetWorldsAsync()
        {
            try
            {
                StorageFolder worldsDir = await StorageFolder.GetFolderFromPathAsync(McWorldsPath);
                return (await worldsDir.GetFoldersAsync()).Select((folder) => new WorldInfo(folder)).ToArray();
            }
            catch
            {
                return new WorldInfo[] { };
            }
        }
    }
}

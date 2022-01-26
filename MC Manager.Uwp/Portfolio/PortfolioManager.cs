using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MC_Manager.Portfolio
{
    public static class PortfolioManager
    {
        public static string ScreenshotsDirPath { get => System.IO.Path.Combine(Utils.UserLocalAppData, @"Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\screenshots\"); }

        public static async Task<PortfolioInfo[]> GetAvailablePortfoliosAsync()
        {
            Worlds.WorldInfo[] worlds = await Worlds.WorldManager.GetWorldsAsync();
            string[] worldIds = worlds.Select((x) => x.Id).ToArray();

            string path = ScreenshotsDirPath;
            StorageFolder screenshotsDir = await StorageFolder.GetFolderFromPathAsync(path);
            return (await screenshotsDir.GetFoldersAsync()).Select((dir) =>
            {
                if (worldIds.Contains(dir.Name))
                    return new PortfolioInfo(dir, worlds.First((x) => x.Id == dir.Name));

                return new PortfolioInfo(dir);
            }).ToArray();
        }
    }
}
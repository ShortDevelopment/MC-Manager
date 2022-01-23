using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MC_Manager.Logs
{
    public static class LogManager
    {
        public static string McClientLogPath { get => System.IO.Path.Combine(Utils.UserLocalAppData, @"Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\logs\"); }
        public static string McServerLogPath { get => System.IO.Path.GetFullPath(System.IO.Path.Combine(Utils.UserLocalAppData, @"..\Roaming\logs")); }

        public static async Task<LogInfo[]> GetAvailableLogsAsync(string dirPath)
        {
            StorageFolder screenshotsDir = await StorageFolder.GetFolderFromPathAsync(dirPath);
            return (await screenshotsDir.GetFilesAsync()).Select((file) => new LogInfo(file)).ToArray();
        }
    }
}

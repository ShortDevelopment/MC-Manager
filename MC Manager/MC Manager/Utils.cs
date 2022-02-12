using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace MC_Manager
{
    public static class Utils
    {
        public static void Await(this IAsyncAction asyncAction)
        {
            asyncAction.GetAwaiter().GetResult();
        }
        public static T Await<T>(this IAsyncOperation<T> asyncOperation)
        {
            return asyncOperation.GetAwaiter().GetResult();
        }
        public static T Await<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }

        public static string UserLocalAppData { get => ApplicationData.Current.LocalFolder.Path.Split("Packages")[0]; }
        public static string MinecraftPath
        {
            set
            {
                var settings = ApplicationData.Current.LocalSettings;
                settings.Values["minecraftPath"] = value;
            }
            get
            {
                var settings = ApplicationData.Current.LocalSettings;
                var result = settings.Values["minecraftPath"] as string;
                if (!string.IsNullOrEmpty(result))
                    return result;
                return Path.Combine(UserLocalAppData, @"Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\");
            }
        }
    }
}

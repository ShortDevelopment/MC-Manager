using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MC_Manager
{
    public static class Utils
    {
        public static IRandomAccessStream OpenSync(this StorageFile file, FileAccessMode accessMode)
        {
            Task<IRandomAccessStream> task = file.OpenAsync(accessMode).AsTask();

            while (!task.IsCompleted) { }
            if (task.Exception != null)
                throw task.Exception;

            return task.Result;
        }

        public static StorageFile GetFileSync(this StorageFolder folde, string name)
        {
            Task<StorageFile> task = folde.GetFileAsync(name).AsTask();

            while (!task.IsCompleted) { }
            if (task.Exception != null)
                throw task.Exception;

            return task.Result;
        }

        public static string UserLocalAppData { get => ApplicationData.Current.LocalFolder.Path.Split("Packages")[0]; }
    }
}

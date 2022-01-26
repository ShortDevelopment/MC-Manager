using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;

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
    }
}

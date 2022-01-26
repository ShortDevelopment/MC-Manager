using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MC_Manager.Logs
{
    public class LogInfo
    {
        public LogInfo(StorageFile file)
        {
            this.File = file;
        }

        public string Name { get => this.File.DisplayName; }
        public StorageFile File { get; private set; }

        public async Task<string> GetContentAsync()
        {
            using (IRandomAccessStream fileStream = await File.OpenReadAsync())
            using (Stream stream = fileStream.AsStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd().Replace("\n", "\r\n");
            }
        }
    }
}

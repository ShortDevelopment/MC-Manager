using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MC_Manager.Worlds
{
    public class WorldInfo
    {
        public WorldInfo(StorageFolder folder)
        {
            this.Folder = folder;
        }

        public string Id { get => Folder.Name; }
        public string Name
        {
            get
            {
                using (IRandomAccessStream fileStream = this.Folder.GetFileAsync("levelname.txt").Await().OpenReadAsync().Await())
                using (Stream stream = fileStream.AsStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        public StorageFolder Folder { get; private set; }

        public ImageSource Image
        {
            get
            {
                using (IRandomAccessStream fileStream = this.Folder.GetFileAsync("world_icon.jpeg").Await().OpenReadAsync().Await())
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    return bitmapImage;
                }
            }
        }
    }
}

using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MC_Manager.Portfolio
{
    public class PortfolioItemInfo
    {
        public PortfolioItemInfo(StorageFile file)
        {
            this.File = file;
        }

        public string Name { get => this.File.DisplayName; }
        public StorageFile File { get; private set; }

        public ImageSource Image
        {
            get
            {
                using (IRandomAccessStream fileStream = File.OpenSync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    return bitmapImage;
                }
            }
        }
    }
}

using System;
using System.Threading.Tasks;
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

        public async Task InitializeAsync()
        {
            using (IRandomAccessStream fileStream = await File.OpenReadAsync())
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.DecodePixelWidth = 100;
                bitmapImage.DecodePixelHeight = 100;
                bitmapImage.SetSource(fileStream);
                this.Image = bitmapImage;
            }
        }

        public ImageSource Image { get; private set; }
    }
}

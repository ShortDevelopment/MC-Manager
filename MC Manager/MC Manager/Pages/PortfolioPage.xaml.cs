using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Microsoft.UI.Xaml.Controls;
using WinUI.Interop.CoreWindow;
using WinUI.Interop.NativeWindow;
using WinRT;

namespace MC_Manager.Pages
{
    public sealed partial class PortfolioPage : Page
    {
        public PortfolioPage()
        {
            this.InitializeComponent();
            Initialize();

            DataTransferManager dataTransferManager = DataTransferManagerInterop.GetForWindow(
                MainWindow.Current.As<IWindowNative>().WindowHandle
            );
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private async void Initialize()
        {
            if (DesignMode.DesignModeEnabled || DesignMode.DesignMode2Enabled)
                return;

            loadingIndicator.Visibility = Microsoft.UI.Xaml.Visibility.Visible;

            try
            {
                this.Portfolios = await Portfolio.PortfolioManager.GetAvailablePortfoliosAsync();
                portfolioSelectListBox.ItemsSource = Portfolios.Select((potfolio) => potfolio.Name).ToArray();
                portfolioSelectListBox.SelectedIndex = 0;
            }
            catch
            {
                MissingPermissionInfo.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }

            loadingIndicator.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        public Portfolio.PortfolioInfo[] Portfolios { get; private set; }

        private async void portfolioSelectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (portfolioSelectListBox.SelectedIndex < 0)
                return;

            loadingIndicator.Visibility = Microsoft.UI.Xaml.Visibility.Visible;

            portfolioItemGridView.ItemsSource = null;

            Portfolio.PortfolioInfo portfolio = this.Portfolios[portfolioSelectListBox.SelectedIndex];

            Portfolio.PortfolioItemInfo[] items = await portfolio.GetItemsAsync();
            await Task.WhenAll(items.Select((item) => item.InitializeAsync()));
            portfolioItemGridView.ItemsSource = items;

            loadingIndicator.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        private async void portfolioItemGridView_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            if (portfolioItemGridView.SelectedIndex < 0)
                return;

            Portfolio.PortfolioItemInfo[] items = portfolioItemGridView.ItemsSource as Portfolio.PortfolioItemInfo[];
            await Launcher.LaunchFileAsync(items[portfolioItemGridView.SelectedIndex].File);
        }

        private async void OpenButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (portfolioItemGridView.SelectedIndex < 0)
                return;

            Portfolio.PortfolioItemInfo[] items = portfolioItemGridView.ItemsSource as Portfolio.PortfolioItemInfo[];
            await Launcher.LaunchFileAsync(items[portfolioItemGridView.SelectedIndex].File);
        }

        private void CopyButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (portfolioItemGridView.SelectedIndex < 0)
                return;

            Portfolio.PortfolioItemInfo[] items = portfolioItemGridView.ItemsSource as Portfolio.PortfolioItemInfo[];
            StorageFile file = items[portfolioItemGridView.SelectedIndex].File;

            DataPackage package = new DataPackage();
            package.RequestedOperation = DataPackageOperation.Copy;
            package.SetBitmap(RandomAccessStreamReference.CreateFromFile(file));

            Clipboard.SetContent(package);
        }

        private void ShareButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            DataTransferManagerInterop.ShowShareUIForWindow(
                MainWindow.Current.As<IWindowNative>().WindowHandle
            );
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            DataRequestDeferral deferral = request.GetDeferral();

            try
            {
                if (portfolioItemGridView.SelectedIndex < 0)
                    throw new NotSupportedException("No item has been chosen!");

                Portfolio.PortfolioItemInfo[] items = portfolioItemGridView.ItemsSource as Portfolio.PortfolioItemInfo[];
                StorageFile file = items[portfolioItemGridView.SelectedIndex].File;

                request.Data.Properties.Title = file.DisplayName;
                request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(file));
            }
            catch (Exception ex)
            {
                request.FailWithDisplayText(ex.Message);
            }

            deferral.Complete();
        }

        private async void DeleteButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (portfolioItemGridView.SelectedIndex < 0)
                return;

            List<Portfolio.PortfolioItemInfo> items = (portfolioItemGridView.ItemsSource as Portfolio.PortfolioItemInfo[]).ToList();

            await items[portfolioItemGridView.SelectedIndex].File.DeleteAsync();

            items.RemoveAt(portfolioItemGridView.SelectedIndex);
            portfolioItemGridView.ItemsSource = items.ToArray();
        }
    }
}

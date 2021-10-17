using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace MC_Manager.Pages
{
    public sealed partial class PortfolioPage : Page
    {
        public PortfolioPage()
        {
            this.InitializeComponent();
            Initialize();

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private async void Initialize()
        {
            if (DesignMode.DesignModeEnabled || DesignMode.DesignMode2Enabled)
                return;

            loadingIndicator.Visibility = Windows.UI.Xaml.Visibility.Visible;

            this.Portfolios = await Portfolio.PortfolioManager.GetAvailablePortfoliosAsync();
            portfolioSelectListBox.ItemsSource = Portfolios.Select((potfolio) => potfolio.Name).ToArray();
            portfolioSelectListBox.SelectedIndex = 0;

            loadingIndicator.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public Portfolio.PortfolioInfo[] Portfolios { get; private set; }

        private async void portfolioSelectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (portfolioSelectListBox.SelectedIndex < 0)
                return;

            loadingIndicator.Visibility = Windows.UI.Xaml.Visibility.Visible;

            portfolioItemGridView.ItemsSource = null;

            Portfolio.PortfolioInfo portfolio = this.Portfolios[portfolioSelectListBox.SelectedIndex];
            portfolioItemGridView.ItemsSource = await portfolio.GetItemsAsync();

            loadingIndicator.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void portfolioItemGridView_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            if (portfolioItemGridView.SelectedIndex < 0)
                return;

            Portfolio.PortfolioItemInfo[] items = portfolioItemGridView.ItemsSource as Portfolio.PortfolioItemInfo[];
            await Launcher.LaunchFileAsync(items[portfolioItemGridView.SelectedIndex].File);
        }

        private async void OpenButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (portfolioItemGridView.SelectedIndex < 0)
                return;

            Portfolio.PortfolioItemInfo[] items = portfolioItemGridView.ItemsSource as Portfolio.PortfolioItemInfo[];
            await Launcher.LaunchFileAsync(items[portfolioItemGridView.SelectedIndex].File);
        }

        private async void CopyButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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

        private async void ShareButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
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

        private async void DeleteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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

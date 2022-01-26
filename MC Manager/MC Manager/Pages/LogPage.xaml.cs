using System.Linq;
using Windows.ApplicationModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace MC_Manager.Pages
{
    public sealed partial class LogPage : Page
    {
        public LogPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string logDirPath = e.Parameter as string;
            Initialize(logDirPath);
        }

        private async void Initialize(string logDirPath)
        {
            if (DesignMode.DesignModeEnabled || DesignMode.DesignMode2Enabled)
                return;

            try
            {
                this.LogInfos = await Logs.LogManager.GetAvailableLogsAsync(logDirPath);
                logSelectListBox.ItemsSource = LogInfos.Select((potfolio) => potfolio.Name).ToArray();
                logSelectListBox.SelectedIndex = 0;
            }
            catch
            {
                MissingPermissionInfo.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }
        }

        public Logs.LogInfo[] LogInfos { get; private set; }

        private async void logSelectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (logSelectListBox.SelectedIndex < 0)
                return;

            Logs.LogInfo logInfo = this.LogInfos[logSelectListBox.SelectedIndex];

            LogTextBox.Text = "";
            string content = await logInfo.GetContentAsync();
            LogTextBox.Text = content;
        }
    }
}

using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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

            this.LogInfos = await Logs.LogManager.GetAvailableLogsAsync(logDirPath);
            logSelectListBox.ItemsSource = LogInfos.Select((potfolio) => potfolio.Name).ToArray();
            logSelectListBox.SelectedIndex = 0;
        }

        public Logs.LogInfo[] LogInfos { get; private set; }

        private void logSelectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (logSelectListBox.SelectedIndex < 0)
                return;

            Logs.LogInfo logInfo = this.LogInfos[logSelectListBox.SelectedIndex];

            LogTextBox.Text = "";
            LogTextBox.Text = logInfo.Content;
        }
    }
}

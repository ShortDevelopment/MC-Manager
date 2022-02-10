using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using WinRT;
using WinUI.Interop.NativeWindow;

namespace MC_Manager
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            Current = this;

            // https://docs.microsoft.com/de-de/windows/apps/design/style/mica

            IntPtr hWnd = MainWindow.Current.As<IWindowNative>().WindowHandle;
            var winId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow window = AppWindow.GetFromWindowId(winId);
            var titleBar = window.TitleBar;
            if (titleBar != null)
            {
                titleBar.ExtendsContentIntoTitleBar = true;
                titleBar.BackgroundColor = Colors.Transparent;
                titleBar.InactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
            else
            {
                ExtendsContentIntoTitleBar = true;
            }

            this.InitializeComponent();
        }

        public static new MainWindow Current { get; private set; }
    }
}

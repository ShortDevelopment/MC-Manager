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
            this.InitializeComponent();

            // https://docs.microsoft.com/de-de/windows/apps/design/style/mica
            IntPtr hWnd = this.As<IWindowNative>().WindowHandle;
            var winId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow window = AppWindow.GetFromWindowId(winId);
            window.Title = "MC Manager";
        }

        public static new MainWindow Current { get; private set; }
    }
}

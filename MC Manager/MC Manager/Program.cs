using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MC_Manager
{
    public static class Programx

    {
        [DllImport("Microsoft.ui.xaml.dll")]
        private static extern void XamlCheckProcessRequirements();

        [STAThreadAttribute]
        static void Mainx(string[] args)
        {
            // XamlCheckProcessRequirements();

            //global::WinRT.ComWrappersSupport.InitializeComWrappers();
            //global::Microsoft.UI.Xaml.Application.Start((p) => {
            //    var context = new global::Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(global::Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
            //    global::System.Threading.SynchronizationContext.SetSynchronizationContext(context);
            //    new App();
            //});
        }
    }
}

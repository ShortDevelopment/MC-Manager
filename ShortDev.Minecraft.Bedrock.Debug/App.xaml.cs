using System;
using System.Windows;

namespace ShortDev.Minecraft.Bedrock.Debug
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            WebServer.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // ToDo: Stop Server
        }
    }
}

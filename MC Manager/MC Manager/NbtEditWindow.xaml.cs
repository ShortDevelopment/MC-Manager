using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using ShortDev.Minecraft.Nbt;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace MC_Manager
{
    public sealed partial class NbtEditWindow : Window
    {
        public NbtEditWindow(StorageFolder worldLocation)
        {
            this.InitializeComponent();

            _ = page.LoadDataAsync(worldLocation);
        }
    }
}

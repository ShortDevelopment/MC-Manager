using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using ShortDev.Minecraft.Nbt;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace MC_Manager.Pages
{
    public sealed partial class NbtEditPage : Page
    {
        public NbtEditPage()
        {
            this.InitializeComponent();
        }

        public async Task LoadDataAsync(StorageFolder worldLocation)
        {
            var file = await worldLocation.GetFileAsync("level.dat");
            var tag = NbtConvert.Convert(file.Path, useGzip: false);
            NbtTreeView.ItemsSource = tag.Children;
        }
    }
}

namespace MC_Manager.Nbt
{
    public class NbtItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ContainerTemplate { get; set; }
        public DataTemplate CollectionTemplate { get; set; }
        public DataTemplate NumberTemplate { get; set; }
        public DataTemplate StringTemplate { get; set; }

        public DataTemplate ItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is NbtTag nbtTag)
            {
                switch (nbtTag.Type)
                {
                    case NbtTagType.String:
                        return StringTemplate;
                    case NbtTagType.Compound:
                        return ContainerTemplate;
                    case NbtTagType.ByteArray:
                    case NbtTagType.IntArray:
                    case NbtTagType.LongArray:
                    case NbtTagType.List:
                        return CollectionTemplate;
                    default:
                        return NumberTemplate;
                }
            }
            else
                return ItemTemplate;
        }
    }

    public class Object2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => value?.ToString() ?? "null";

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => value?.ToString() ?? "null";
    }
}
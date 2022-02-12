using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Text;

namespace MC_Manager.Controls
{
    public static class McFormattedTextBlock
    {
        public static string GetMcFormattedText(DependencyObject obj)
            => (string)obj.GetValue(McFormattedTextProperty);

        public static void SetMcFormattedText(DependencyObject obj, string value)
            => obj.SetValue(McFormattedTextProperty, value);

        public static readonly DependencyProperty McFormattedTextProperty =
            DependencyProperty.RegisterAttached(
                "McFormattedText",
                typeof(string),
                typeof(McFormattedTextBlock),
                new PropertyMetadata(null, OnChanged)
            );

        static Dictionary<char, string> mcColors = new()
        {
            { '0', "#000000" },
            { '1', "#0000AA" },
            { '2', "#00AA00" },
            { '3', "#00AAAA" },
            { '4', "#AA0000" },
            { '5', "#AA00AA" },
            { '6', "#FFAA00" },
            { '7', "#AAAAAA" },
            { '8', "#555555" },
            { '9', "#5555FF" },
            { 'a', "#55FF55" },
            { 'b', "#55FFFF" },
            { 'c', "#FF5555" },
            { 'd', "#FF55FF" },
            { 'e', "#FFFF55" },
            { 'f', "#FFFFFF" },
            { 'g', "#FFFFFF" }
        };

        private static void OnChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var target = dependencyObject as TextBlock;
            if (target != null)
            {
                target.Inlines.Clear();

                string input = e.NewValue as string;

                Run lastInline = null;
                if (!input.Contains('§'))
                    target.Inlines.Add(new Run() { Text = input });
                else
                    foreach (string section in input.Split('§'))
                    {
                        if (section.Length == 0)
                            continue;

                        char ctlChar = section[0];

                        Run currentInline = new();
                        if (section.Length > 1)
                            currentInline.Text = section.Substring(1);

                        if (ctlChar == 'r' /* reset */)
                            lastInline = null;

                        if (lastInline != null)
                        {
                            currentInline.Foreground = lastInline.Foreground;
                            currentInline.FontWeight = lastInline.FontWeight;
                            currentInline.TextDecorations = lastInline.TextDecorations;
                            currentInline.FontStyle = lastInline.FontStyle;
                        }

                        switch (ctlChar)
                        {
                            case 'k': // obfuscated
                                currentInline.Text = "".PadLeft(section.Length - 1, 'Ā');
                                break;
                            case 'l': // bold
                                currentInline.FontWeight = FontWeights.Bold;
                                break;
                            case 'm': // strikethrough
                                currentInline.TextDecorations = TextDecorations.Strikethrough;
                                break;
                            case 'n': // underline
                                currentInline.TextDecorations = TextDecorations.Underline;
                                break;
                            case 'o': // italic
                                currentInline.FontStyle = FontStyle.Italic;
                                break;

                            default: // colors
                                if (mcColors.ContainsKey(ctlChar))
                                    currentInline.Foreground = new SolidColorBrush((Color)XamlBindingHelper.ConvertValue(typeof(Color), mcColors[ctlChar]));
                                break;
                        }

                        lastInline = currentInline;
                        target.Inlines.Add(currentInline);
                    }
            }
        }
    }
}

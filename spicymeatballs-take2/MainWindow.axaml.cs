using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace spicymeatballs_take2
{
    public partial class MainWindow : Window
    {
        private int _width, _height;
        private List<int> _pixels = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnLoadClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filters = { new FileDialogFilter { Name = "B2Img Files", Extensions = { "txt" } } } };
            var result = await dialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                LoadImage(result[0]);
            }
        }

        private void LoadImage(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var size = lines[0].Split(" ");
            _height = int.Parse(size[0]);
            _width = int.Parse(size[1]);

            _pixels = lines[1].Select(c => c - '0').ToList();

            RenderImage();
        }

        private void RenderImage()
        {
            ImageGrid.ItemsSource = _pixels.Select((value, index) =>
            {   
                var button = new Button
                {
                    Background = value == 1 ? Avalonia.Media.Brushes.Black : Avalonia.Media.Brushes.White,
                    Tag = index
                };
                button.Click += PixelClicked;
                return button;
            }).ToList();
        }

        private void PixelClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int index)
            {
                _pixels[index] = _pixels[index] == 1 ? 0 : 1;
                button.Background = _pixels[index] == 1 ? Avalonia.Media.Brushes.Black : Avalonia.Media.Brushes.White;
            }
        }

        private async void OnSaveClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { DefaultExtension = "txt" };
            var result = await dialog.ShowAsync(this);
            if (!string.IsNullOrEmpty(result))
            {
                File.WriteAllLines(result, new[]
                {
                    $"{_height} {_width}",
                    string.Join("", _pixels)
                });
            }
        }
    }
}

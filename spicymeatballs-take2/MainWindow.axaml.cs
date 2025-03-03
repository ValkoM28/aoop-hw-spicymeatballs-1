using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Layout;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform.Storage;
namespace spicymeatballs_take2;

public partial class MainWindow : Window
{
    private IBrush? color = Brushes. Black;
    private int width, height;
    private int [,]image;
    private int pixelSize;

    
    public MainWindow()
    {
        InitializeComponent();
    }
    
    // Function for Load Button
      private async void load(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            
            var load = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Open Text File",
                    AllowMultiple = false
                });

            if (load.Count >= 1)
            {
                // Open reading stream from the first file.
                await using var stream = await load[0].OpenReadAsync();
                using var streamReader = new StreamReader(stream);
                // Reads all the content of file as a text.
                var fileContent = await streamReader.ReadToEndAsync();
                string[] data = fileContent.Split(' ');
                SetCanvas(data);
            }
        }
        public void SetCanvas(string[] lines)
        {
           /* if  (lines.Length < 2)
            {
                throw new Exception("error, not enough info");
            }
            string[] size = lines[0].Split(' ');
            if (size.Length < 2)
            {
                throw new Exception("error, no height or width");
            }
            if (!int.TryParse(size[0],out width)||!int.TryParse(size[1],out height) || width < 1 || height < 1)
            {
                throw new Exception("You suck");
            } */

            Canvas.Children.Clear();
            Canvas.Height = height *pixelSize;
            Canvas.Width = Width * pixelSize;
            image = new int[height, width];

            for (int row =0, lineIndex = 1; row< height && lineIndex < lines.Length; row++, lineIndex++)
            {
                string line =lines[lineIndex].Replace("","");
                if (line.Length != width) continue;

                for(int col = 0; col < width;col++)
                {
                    image[row, col] = line[col] =='1'? 1 : 0;
                    Drawing(row, col);
                }
            }
        } 

        private void Drawing(int x, int y)
        {
            var pixel = new Avalonia.Controls.Shapes.Rectangle()
            {
                Width = pixelSize,
                Height = pixelSize,
                Fill = image[x, y] == 1? color :Brushes.White,
            };
            Canvas.SetLeft(pixel, y*pixelSize);
            Canvas.SetTop(pixel, x*pixelSize);
            Canvas.Children.Add(pixel);
        }        
     private void RenderImageGrid(int width, int height, string pixelData)
    {
        ImageGrid.Children.Clear();
        ImageGrid.ColumnDefinitions.Clear();
        ImageGrid.RowDefinitions.Clear();

        // Define columns and rows
        for (int i = 0; i < width; i++)
            ImageGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        for (int i = 0; i < height; i++)
            ImageGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

        // Populate grid with rectangles
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                int index = row * width + col;
                if (index >= pixelData.Length) break;

                var rect = new Border
                {
                    Background = pixelData[index] == '1' ? Brushes.Black : Brushes.White,
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new (0.5)
                };

                Grid.SetRow(rect, row);
                Grid.SetColumn(rect, col);
                ImageGrid.Children.Add(rect);
            }
        }
    }
    private void Press(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var position = e.GetPosition(Canvas);
        int x = (int)(position.Y/ pixelSize);
        int y = (int)(position.X/ pixelSize);

        if (x >= 0 && x < height && y >= 0 && y < width)
        {
            image[x, y] = image[x, y] == 1 ? 0 : 1;
            Drawing(x, y);
        }
    }
    private async void SaveFile(object? sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Title = "Save your file" };
            var result = await dialog.ShowAsync(this);
        }
        // Helper function to show a simple message box
        private async void ShowMessage(string message)
        {
            var dialog = new Window
            {
                Content = new TextBlock { Text = message, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center },
                Width = 250,
                Height = 100
            };
            await dialog.ShowDialog(this);
        }
}


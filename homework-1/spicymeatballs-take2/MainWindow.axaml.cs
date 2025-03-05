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
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using Avalonia.Media;
namespace spicymeatballs_take2;

public partial class MainWindow : Window
{
    private IBrush? color = Brushes. Black;

    private int _width;
    private int _height;
    private string _imageData;
    
    public string[] ImageData
    {
        get
        {
            return new string[] { _height + " " + _width, _imageData };  //TODO: needs a rework, do not use
        }
        
        set
        {
            string[] temp = value[0].Split(" "); 
            _height = int.Parse(temp[0]);
            _width = int.Parse(temp[1]);
            
            _imageData = value[1]; 
            if (_imageData.Length != _height * _width)
            {
                throw new Exception("not matching width and height to pixel count");
            }
            if (_height < 2 || _width < 2)
            {
                throw new Exception("error, not enough info");
            }
            
            SetCanvas(_height, _width, _imageData); 
            
        }
    }

    
    public MainWindow()
    {
        InitializeComponent();
    }
    
    // Function for Load Button
      private async void LoadImage(object? sender, RoutedEventArgs e)
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
                string[] data = fileContent.Split('\n');
                this.ImageData = data;
                Console.WriteLine("data loaded");
                Console.WriteLine(data[0] + " " + data[1]);
                
                //TODO: add type checking
            }
            
        }

        public void SetCanvas(int imageHeight, int imageWidth, string imageData)
        {
            CanvasMain.Children.Clear(); // Clear previous content

            int pixelSize = 10; // Define pixel size
            CanvasMain.Width = imageWidth * pixelSize;
            CanvasMain.Height = imageHeight * pixelSize;

            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    char pixel = imageData[i * imageWidth + j];

                    // Convert '1' to black and '0' to white
                    var brush = (pixel == '1') ? Brushes.Black : Brushes.White;

                    // Create a rectangle
                    var rect = new Rectangle
                    {
                        Width = pixelSize,
                        Height = pixelSize,
                        Fill = brush
                    };

                    // Position it on the canvas
                    Canvas.SetLeft(rect, j * pixelSize);
                    Canvas.SetTop(rect, i * pixelSize);

                    // Add to the canvas
                    CanvasMain.Children.Add(rect);
                }
            }

            Console.WriteLine($"Canvas set with {imageHeight}x{imageWidth} pixels.");
        }

        public void TemporaryMessage(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Console.WriteLine("Fuck you");
        }
        public void TemporaryMessage2(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Fuck you");
        }
        /*
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
        }   */     
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
    /*
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
    } */
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


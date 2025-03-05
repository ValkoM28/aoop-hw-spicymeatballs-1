using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Layout;
using System;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform.Storage;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using Avalonia.Media;
using Path = System.IO.Path;
using System.Collections.Generic;


namespace spicymeatballs_take2;

public partial class MainWindow : Window
{
    private IBrush? color = Brushes. Black;
    private int _pixelSize = 10; // Define pixel size

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
            
            if (topLevel == null)
            {
                Console.WriteLine("Error: Unable to get TopLevel window.");
                return;
            }

            var loadFileOptions = new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false,
            };

            
            var load = await topLevel.StorageProvider.OpenFilePickerAsync(loadFileOptions);

            if (load.Count >= 1)
            {
                string filePath = load[0].Path.AbsoluteUri;
                // Open reading stream from the first file.
                await using var stream = await load[0].OpenReadAsync();
                using var streamReader = new StreamReader(stream);

                
                // Reads all the content of file as a text.
                string fileContent = await streamReader.ReadToEndAsync();
                string[] data = fileContent.Split('\n');

                this.ImageData = data;
                Console.WriteLine("data loaded");
                Console.WriteLine(data[0] + " " + data[1]);
                if (IsValid(data, filePath))
                {
                    Console.WriteLine("Data is valid");
                }
                else
                {
                    Console.WriteLine("Data is not valid");
                }
            }
            
        }

        private bool IsValid(string[] data, string filePath)
        {
            if (filePath.EndsWith(".b2img.txt"))
            {
                string allowedCharacters = "01";
                foreach (char c in data[1])
                {
                    if (!allowedCharacters.Contains(c))
                    {
                        return false;
                    }
                }
            } else if (filePath.EndsWith(".b16img.txt"))
            {
                string allowedCharacters = "0123456789abcdef";
                foreach (char c in data[1])
                {
                    if (!allowedCharacters.Contains(c))
                    {
                        return false;
                    }
                }
            }
            else
            {
                throw new Exception("error, wrong file extension");
            }

            Console.WriteLine($"Data is valid { filePath }");
            return true; 
        }

        public void SetCanvas(int imageHeight, int imageWidth, string imageData)
        {
            CanvasMain.Children.Clear(); // Clear previous content

            _pixelSize = (int)Math.Min(CanvasMain.Bounds.Width / imageWidth, CanvasMain.Bounds.Height / imageHeight);


            CanvasMain.Width = imageWidth * _pixelSize;
            CanvasMain.Height = imageHeight * _pixelSize;

            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    char pixel = imageData[i * imageWidth + j];

                    // Define the color mapping
                    var colorMap = new Dictionary<char, IBrush>
                    {
                        { '0', Brushes.White }, { '1', Brushes.Black }, { '2', Brushes.Red }, { '3', Brushes.Green },
                        { '4', Brushes.Blue }, { '5', Brushes.Yellow }, { '6', Brushes.Purple }, { '7', Brushes.Orange },
                        { '8', Brushes.Pink }, { '9', Brushes.Brown }, { 'A', Brushes.Gray }, { 'B', Brushes.Cyan },
                        { 'C', Brushes.Magenta }, { 'D', Brushes.Lime }, { 'E', Brushes.Teal }, { 'F', Brushes.Gold }
                    };

                    // Get the corresponding brush for the pixel character
                    var brush = colorMap.ContainsKey(pixel) ? colorMap[pixel] : Brushes.White;

                    // Create a rectangle
                    var rect = new Rectangle
                    {
                        Width = _pixelSize,
                        Height = _pixelSize,
                        Fill = brush
                    };

                    // Position it on the canvas
                    Canvas.SetLeft(rect, j * _pixelSize);
                    Canvas.SetTop(rect, i * _pixelSize);

                    // Add to the canvas
                    CanvasMain.Children.Add(rect);
                }
            }

            Console.WriteLine($"Canvas set with {imageHeight}x{imageWidth} pixels.");
        }

        private bool _isFlippedHorizontally = false;
        private bool _isFlippedVertically = false;

        public void FlipHorizontal(object? sender, RoutedEventArgs e)
        {
            var children = CanvasMain.Children.OfType<Rectangle>().ToList();
            CanvasMain.Children.Clear();

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    var rect = children[i * _width + j];

                    // If already flipped horizontally, reverse the flip, else flip horizontally
                    if (_isFlippedHorizontally)
                    {
                        // Reverse horizontal flip
                        Canvas.SetLeft(rect, j * _pixelSize);
                    }
                    else
                    {
                        // Flip horizontally
                        Canvas.SetLeft(rect, (_width - 1 - j) * _pixelSize);
                    }

                    Canvas.SetTop(rect, i * _pixelSize);
                    CanvasMain.Children.Add(rect);
                }
            }

            // Toggle the flip state
            _isFlippedHorizontally = !_isFlippedHorizontally;

            Console.WriteLine(_isFlippedHorizontally ? "Canvas flipped horizontally." : "Canvas flipped back horizontally.");
        }

        public void FlipVertical(object? sender, RoutedEventArgs e)
        {
            var children = CanvasMain.Children.OfType<Rectangle>().ToList();
            CanvasMain.Children.Clear();

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    var rect = children[i * _width + j];

                    // If already flipped vertically, reverse the flip, else flip vertically
                    if (_isFlippedVertically)
                    {
                        // Reverse vertical flip
                        Canvas.SetTop(rect, i * _pixelSize);
                    }
                    else
                    {
                        // Flip vertically
                        Canvas.SetTop(rect, (_height - 1 - i) * _pixelSize);
                    }

                    Canvas.SetLeft(rect, j * _pixelSize);
                    CanvasMain.Children.Add(rect);
                }
            }

            // Toggle the flip state
            _isFlippedVertically = !_isFlippedVertically;

            Console.WriteLine(_isFlippedVertically ? "Canvas flipped vertically." : "Canvas flipped back vertically.");
        }

       public void ColorFlip(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            // Get the position of the click relative to the canvas
            var position = e.GetPosition(CanvasMain);

            // Calculate the pixel's x and y coordinates based on _pixelSize
            int x = (int)(position.Y / _pixelSize);
            int y = (int)(position.X / _pixelSize);

            // If the image is flipped horizontally, adjust the y coordinate
            if (_isFlippedHorizontally)
            {
                y = _width - 1 - y;
            }

            // If the image is flipped vertically, adjust the x coordinate
            if (_isFlippedVertically)
            {
                x = _height - 1 - x;
            }

            // Check if the clicked position is within bounds of the image
            if (x >= 0 && x < _height && y >= 0 && y < _width)
            {
                // Find the rectangle at the corrected position
                int index = x * _width + y;
                var rect = CanvasMain.Children.OfType<Rectangle>().ElementAtOrDefault(index);

                if (rect != null)
                {
                    // Define the color sequence
                    IBrush[] colors = new IBrush[]
                    {
                        Brushes.White, Brushes.Black, Brushes.Red, Brushes.Green, Brushes.Blue,
                        Brushes.Yellow, Brushes.Purple, Brushes.Orange, Brushes.Pink, Brushes.Brown,
                        Brushes.Gray, Brushes.Cyan, Brushes.Magenta, Brushes.Lime, Brushes.Teal, Brushes.Gold
                    };

                    // Find the current color index
                    int currentIndex = Array.IndexOf(colors, rect.Fill);

                    // Calculate the next color index
                    int nextIndex = (currentIndex + 1) % colors.Length;

                    // Set the rectangle's fill to the next color
                    rect.Fill = colors[nextIndex];

                    Console.WriteLine($"Flipped color at ({x}, {y}) to {colors[nextIndex]}");
                }
            }
        }


        public void TemporaryMessage(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Console.WriteLine("Fuck you");
        }
        public void TemporaryMessage2(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Fuck you");
        }
    private async void SaveFile(object? sender, RoutedEventArgs e)
    {
        var data = CanvasMain.Children.OfType<Rectangle>();
        string outputString = $"{_height} {_width}\n";
        string dataOutput = "";
        
        // Define the color mapping
        var colorMap = new Dictionary<IBrush, char>
        {
            { Brushes.White, '0' }, { Brushes.Black, '1' }, { Brushes.Red, '2' }, { Brushes.Green, '3' },
            { Brushes.Blue, '4' }, { Brushes.Yellow, '5' }, { Brushes.Purple, '6' }, { Brushes.Orange, '7' },
            { Brushes.Pink, '8' }, { Brushes.Brown, '9' }, { Brushes.Gray, 'A' }, { Brushes.Cyan, 'B' },
            { Brushes.Magenta, 'C' }, { Brushes.Lime, 'D' }, { Brushes.Teal, 'E' }, { Brushes.Gold, 'F' }
        };

        foreach (var item in data) 
        {
            var brush = item.Fill;
            if (colorMap.ContainsKey(brush))
            {
                dataOutput += colorMap[brush];
            }
            else
            {
                dataOutput += '0'; // Default to white if color not found
            }
        }
        char[] uniqueChars = dataOutput.Distinct().ToArray();
        string extension;
        if (uniqueChars.Length == 2)
        {
            extension = ".b2img.txt";
        } else {
            extension = ".b16img.txt";
        }

        outputString += dataOutput;
        
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            Console.WriteLine("Error: Unable to get TopLevel window.");
            return;
        }

        var saveFileOptions = new FilePickerSaveOptions
        {
            Title = "Save File",
            SuggestedFileName = "output" + extension,
            DefaultExtension = "b2img.txt",
            ShowOverwritePrompt = true
        };

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(saveFileOptions);
        if (file != null)
        {
            await using var stream = await file.OpenWriteAsync();
            using var writer = new StreamWriter(stream);
            await writer.WriteAsync(outputString);
        }
    }
    
}


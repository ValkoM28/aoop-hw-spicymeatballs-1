using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.IO;
using System.Linq;
using Avalonia.Platform.Storage;
using Avalonia.Controls.Shapes;
using System.Collections.Generic;


namespace spicymeatballs_take2;

public partial class MainWindow : Window
{
    private const int DefaultPixelSize = 10;

    private int _pixelSize = DefaultPixelSize;
    private int _width;
    private int _height;
    private string _imageData;

    private static readonly Dictionary<char, IBrush> ColorMap = new()
    {
        { '0', Brushes.White }, { '1', Brushes.Black }, { '2', Brushes.Red }, { '3', Brushes.Green },
        { '4', Brushes.Blue }, { '5', Brushes.Yellow }, { '6', Brushes.Purple }, { '7', Brushes.Orange },
        { '8', Brushes.Pink }, { '9', Brushes.Brown }, { 'A', Brushes.Gray }, { 'B', Brushes.Cyan },
        { 'C', Brushes.Magenta }, { 'D', Brushes.Lime }, { 'E', Brushes.Teal }, { 'F', Brushes.Gold }
    };

    private static readonly Dictionary<IBrush, char> ReverseColorMap = ColorMap.ToDictionary(kv => kv.Value, kv => kv.Key);
    private static readonly IBrush[] Colors = ReverseColorMap.Keys.ToArray();


    public string[] ImageData
    {
        get
        {
            /*
             * This getter reads the canvas rather than the _imageData field, because the canvas can be manipulated without updating the _imageData field.
             * The getter is used to save the image data and also to perform internal logic like flipping the image.
             */
            string dimensions = $"{_height} {_width}\n";
            var data = CanvasMain.Children.OfType<Rectangle>();
            string dataOutput = "";


            foreach (var item in data)
            {
                var brush = item.Fill;
                if (ReverseColorMap.ContainsKey(brush))
                {
                    dataOutput += ReverseColorMap[brush];
                }
                else
                {
                    dataOutput += '0'; // Default to white if color not found
                }

            }

            return [dimensions, dataOutput];
        }

        set
        {
            /*
             * The setter is used to update image fields and rerender the image with the new data.
             */
            if (value.Length < 2)
            {
                Console.WriteLine("Error: Invalid image data.");
                return;
            }

            if (!ParseDimensions(value[0], out _height, out _width))
            {
                Console.WriteLine("Error: Invalid image dimensions setup.");
                return;
            }

            _imageData = value[1];

            if (_imageData.Length != _height * _width)
            {
                Console.WriteLine("Error: Number of pixels does not match dimensions.");
                return;
            }

            SetCanvas(_height, _width, _imageData);

        }
    }
    
    public MainWindow()
    {
        InitializeComponent();
    }

    /*
     * File handling logic here
     */

    private async void LoadImage(object? sender, RoutedEventArgs e)
    {
        try
        {
            var topLevel = TopLevel.GetTopLevel(this);

            if (topLevel == null)
            {
                Console.WriteLine("Error: Unable to get the explorer window.");
                return;
            }

            var loadFileOptions = new FilePickerOpenOptions
            {
                Title = "Open a File",
                AllowMultiple = false,
            };


            var load = await topLevel.StorageProvider.OpenFilePickerAsync(loadFileOptions);

            if (load.Count == 0)
            {
                Console.WriteLine("No file selected.");
                return;
            }

            string filePath = load[0].Path.LocalPath;

            // Open reading stream from the first file.
            await using var stream = await load[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);


            // Reads all the content of file as a text.
            string fileContent = await streamReader.ReadToEndAsync();
            string[] data = fileContent.Split('\n');


            if (!FileContentCheck(data, filePath))
            {
                Console.WriteLine("Error: File format incorrect, invalid data.");
                return;
            }

            this.ImageData = data;

            Console.WriteLine("data loaded");
            Console.WriteLine(data[0] + " " + data[1]);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Caught: " + ex.Message);
        }
    }

    private bool FileContentCheck(string[] data, string filePath)
    {
        try
        {
            if (data.Length < 2)
            {
                Console.WriteLine("Error: missing data.");
                return false;
            }

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
            }
            else if (filePath.EndsWith(".b16img.txt"))
            {
                foreach (char c in data[1])
                {
                    if (!Uri.IsHexDigit(c))
                    {
                        return false;
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Unsupported file extension.");
                return false;
            }

            Console.WriteLine($"Data is valid {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Caught: " + ex.Message);
            return false;
        }
    }

    private async void SaveFile(object? sender, RoutedEventArgs e)
    {

        var data = CanvasMain.Children.OfType<Rectangle>();
        if (!data.Any())
        {
            Console.WriteLine("Error: No image data to save.");
            return;
        }

        string[] imageData = ImageData;
        string dimensions = imageData[0];
        string colorString = imageData[1];
        string outputString = dimensions + colorString;

        string fileExtension;

        if (colorString.Distinct().ToArray().SequenceEqual(new[] { '0', '1' }) ||
            colorString.Distinct().ToArray().SequenceEqual(new[] { '1', '0' }))
        {
            fileExtension = ".b2img.txt";
        }
        else
        {
            fileExtension = ".b16img.txt";
        }

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            Console.WriteLine("Error: Unable to get TopLevel window.");
            return;
        }

        var saveFileOptions = new FilePickerSaveOptions
        {
            Title = "Save File",

            SuggestedFileName = "output" + fileExtension,

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

    
    /*
     * Canvas rendering logic here
     */

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

                // Get the corresponding brush for the pixel character
                var brush = ColorMap.ContainsKey(pixel) ? ColorMap[pixel] : Brushes.White;

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


    /*
     * Image manipulation logic here
     */ 
    public void FlipHorizontal(object? sender, RoutedEventArgs e)
    {
        _imageData = ImageData[1];
        var imageDataArray = _imageData.ToCharArray();

        for (int row = 0; row < _height; row++)
        {
            // Reverse the order of pixels in each row
            for (int col = 0; col < _width / 2; col++)
            {
                // Swap the left and right pixel in the current row
                int leftIndex = row * _width + col;
                int rightIndex = row * _width + (_width - col - 1);

                // Swap pixel values in the image data array
                (imageDataArray[leftIndex], imageDataArray[rightIndex]) =
                    (imageDataArray[rightIndex], imageDataArray[leftIndex]);
            }
        }

        // Update the image data after flipping
        ImageData = [ImageData[0], new string(imageDataArray)];

        // Re-render the canvas with the updated flipped data

        Console.WriteLine("Image flipped horizontally.");
    }

    // Function to flip the image vertically (flip along X-axis)
    public void FlipVertical(object? sender, RoutedEventArgs e)
    {

        _imageData = ImageData[1];
        var imageDataArray = _imageData.ToCharArray();

        for (int col = 0; col < _width; col++)
        {
            // Reverse the order of pixels in each column
            for (int row = 0; row < _height / 2; row++)
            {
                // Swap the top and bottom pixel in the current column
                int topIndex = row * _width + col;
                int bottomIndex = (_height - row - 1) * _width + col;

                // Swap pixel values in the image data array
                (imageDataArray[topIndex], imageDataArray[bottomIndex]) =
                    (imageDataArray[bottomIndex], imageDataArray[topIndex]);
            }
        }

        // Update the image data after flipping and re-render the canvas
        ImageData = [ImageData[0], new string(imageDataArray)];

        Console.WriteLine("Image flipped vertically.");
    }

    public void ColorFlip(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        // Get the position of the click relative to the canvas
        var position = e.GetPosition(CanvasMain);

        // Calculate the pixel's x and y coordinates based on _pixelSize
        int x = (int)(position.Y / _pixelSize);
        int y = (int)(position.X / _pixelSize);

        // Check if the clicked position is within bounds of the image
        if (x >= 0 && x < _height && y >= 0 && y < _width)
        {
            // Find the rectangle at the corrected position
            int index = x * _width + y;
            var rect = CanvasMain.Children.OfType<Rectangle>().ElementAtOrDefault(index);

            if (rect != null)
            {
                // Find the current color index
                int currentIndex = Array.IndexOf(Colors, rect.Fill);

                // Calculate the next color index
                int nextIndex = (currentIndex + 1) % Colors.Length;

                // Set the rectangle's fill to the next color
                rect.Fill = Colors[nextIndex];

                Console.WriteLine($"Flipped color at ({x}, {y}) to {Colors[nextIndex]}");
            }
        }
    }
    
    private static bool ParseDimensions(string dimensionString, out int height, out int width)
    {
        height = 0;
        width = 0;
        var parts = dimensionString.Split(" ");
        if (parts.Length < 2)
        {
            return false;
        }
        return int.TryParse(parts[0], out height) && int.TryParse(parts[1], out width);
    }
}
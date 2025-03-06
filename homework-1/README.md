# Documentation

## LoadImage(object? sender, RoutedEventArgs e)
### Loads an image file and updates the canvas.
- Opens a file dialog.
- Reads the file content.
- Validates the file format.
- Updates the canvas with image data.

## FileContentCheck(string[] data, string filePath)
### Validates the format of the loaded image file.
- Checks for correct file extensions (.b2img.txt and .b16img.txt).
- Ensures pixel data contains valid characters.

### Correct file examples
- black and white

```txt
6 14
010001110000000000010001000010000011111110010001000000000000000000000010000010111110
```
- 16 colors

```txt
6 14
02000100110000101D144420320021221454222110112122151020100200002012120012000120222110
```


## SaveFile(object? sender, RoutedEventArgs e)
### Saves the current image data to a file.
- Determines the appropriate file extension.
- Opens a save file dialog.
- Writes the image data to a file.

## SetCanvas(int imageHeight, int imageWidth, string imageData)
### Renders the canvas based on the given image dimensions and data.
- Clears previous content.
- Calculates pixel sizes.
- Iterates through image data and creates rectangles.

## FlipHorizontal(object? sender, RoutedEventArgs e)
### Flips the image horizontally by reversing pixels in each row.

## FlipVertical(object? sender, RoutedEventArgs e)
### Flips the image vertically by reversing pixels in each column.

## ColorFlip(object? sender, Avalonia.Input.PointerPressedEventArgs e)
### Changes the color of a clicked pixel.
- Determines the clicked pixel's position.
- Finds the current color.
- Changes it to the next color in sequence.

## ParseDimensions(string dimensionString, out int height, out int width)
### Parses the image dimensions from a string and assigns them to height and width.
- Returns false if parsing fails.
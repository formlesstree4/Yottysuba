namespace YottySuba.Components.Services;

public sealed class ImageProcessingService(string rootDirectory)
{
    
    /// <summary>
    /// Attempts to determine if the file given is an image
    /// </summary>
    /// <param name="filePath">The file path as determined from the attachment table</param>
    /// <returns>true if an image</returns>
    /// <remarks>This is in cahoots with the runtime to know what the absolute path is</remarks>
    public async Task<bool> IsFileProbablyAnImage(string filePath)
    {
        try
        {
            await Image.DetectFormatAsync(Path.Combine(rootDirectory, filePath));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
}
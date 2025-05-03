using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Exceptions;

namespace YottySuba.Components.Services;

public sealed class VideoProcessingService(string rootDirectory)
{
    
    public async Task<bool> ConvertToWebM(string file)
    {
        var fullyQualifiedPath = Path.Combine(rootDirectory, file);
        return await FFMpegArguments
            .FromFileInput(fullyQualifiedPath)
            .OutputToFile(Path.ChangeExtension(fullyQualifiedPath, "webm"), false, options => options
                .WithVideoCodec(VideoCodec.LibVpx)
                .WithCustomArgument("-lossless 1")
                .WithConstantRateFactor(10)
                .WithAudioCodec(AudioCodec.LibVorbis)
                .WithVariableBitrate(4)
                .WithVideoFilters(filterOptions => filterOptions
                    .Scale(VideoSize.Hd))
                .WithFastStart()).ProcessAsynchronously();
    }

    public async Task<bool> IsFileProbablyVideo(string file)
    {
        try
        {
            await using var stream = File.OpenRead(Path.Combine(rootDirectory, file));
            var analysis = await FFProbe.AnalyseAsync(stream);
            return analysis.VideoStreams.Count != 0;
        }
        catch (FFMpegException)
        {
            return false;
        }
    }
    
}
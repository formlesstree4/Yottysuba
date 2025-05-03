using System.Security.Cryptography;
using Humanizer;
using Microsoft.AspNetCore.Components.Forms;
using YottySuba.Database;

namespace YottySuba.Components.Services;

public sealed class AttachmentUploadService(string rootFolder, YottysubaContext context)
{
    public async Task<Guid> UploadAttachment(Board board, IBrowserFile attachment)
    {
        var boardName = board.Code;
        var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(attachment.Name)}";
        var maxFileSizeInBytes = board.MaxFilesizeInBytes;
        if (maxFileSizeInBytes <= 0) maxFileSizeInBytes = (long)128.Megabytes().Bytes;
        
        // physical is where it's gonna get written to
        // database is where we lie a little because our file processors are in cahoots with us.
        var physicalFilePath = Path.Combine(rootFolder, "attachments", boardName);
        var databaseFilePath = Path.Combine("attachments", boardName, newFileName);
        await using var sourceStreamData = attachment.OpenReadStream(maxFileSizeInBytes);
        await using var destinationStream = File.Open(Path.Combine(physicalFilePath, newFileName), FileMode.CreateNew);
        await sourceStreamData.CopyToAsync(destinationStream);
        destinationStream.Seek(0, SeekOrigin.Begin); // go back to the beginning for the hash later
        var newAttachment = await context.Attachments.AddAsync(new Attachment
        {
            Location = databaseFilePath,
            Created = DateTime.UtcNow,
            Deleted = false,
            Hash = await SHA512.HashDataAsync(destinationStream),
            Size = attachment.Size
        });
        await context.SaveChangesAsync();
        return newAttachment.Entity.Id;
    }
}
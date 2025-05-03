using System.Text.RegularExpressions;
using AngleSharp;
using Microsoft.EntityFrameworkCore;
using YottySuba.Database;

namespace YottySuba.Components.Services;

public sealed class PostFilterService(YottysubaContext databaseContext)
{
    private static async Task RemoveHtmlFromIncoming(Post post)
    {
        var context = BrowsingContext.New();
        var document = await context.OpenAsync(req => req.Content(post.Message));
        post.Message = document.Body?.TextContent ?? string.Empty;
    }
    
    public async Task ApplyFiltersToPost(Post post)
    {
        var board = await databaseContext.Boards.FirstAsync(b => b.Id == post.Board);
        var filters = await databaseContext
            .Filters
            .Where(f => f.Active && f.Boards!.Contains(board.Id))
            .ToListAsync();
        await RemoveHtmlFromIncoming(post);
        foreach (var filter in filters)
        {
            post.Message = Regex.Replace(post.Message, filter.Expression, filter.Replace);
        }
    }
    
}
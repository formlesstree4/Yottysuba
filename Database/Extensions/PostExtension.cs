using Microsoft.AspNetCore.Components;

namespace YottySuba.Database;

public partial class Post
{

    public string GetBoardAnchorUrl(string code) => $"/boards/{code}#{Id}";
    
    public string GetThreadAnchorUrl(long threadId) => $"/threads/{threadId}#{Id}";

    public string ThreadUrl => $"/threads/{Id}";

    public MarkupString MessageHtml => new MarkupString(Message);

}
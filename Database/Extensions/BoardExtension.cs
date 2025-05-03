using Humanizer;

namespace YottySuba.Database;

public partial class Board
{
    public long MaxFilesizeInBytesOrDefault =>
        MaxFilesizeInBytes <= 0 ? Convert.ToInt64(128.Megabytes().Bytes) : MaxFilesizeInBytes;

    public string BoardUrl => $"/boards/{Code}/";
}
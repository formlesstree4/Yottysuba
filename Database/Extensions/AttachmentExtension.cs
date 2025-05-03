// ReSharper disable once CheckNamespace
namespace YottySuba.Database;

public partial class Attachment
{
    public string Thumbnail => $"{Location}?width=250&format=png";
}
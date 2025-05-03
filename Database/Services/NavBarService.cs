namespace YottySuba.Database.Services;

/// <summary>
/// Provides a dedicated <see cref="YottysubaContext"/> to the Navigation Bar
/// </summary>
/// <param name="context"><see cref="YottysubaContext"/></param>
public sealed class NavBarService (YottysubaContext context)
{
    public YottysubaContext Context => context;
}
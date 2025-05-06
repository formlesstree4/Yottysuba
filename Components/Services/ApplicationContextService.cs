using System.Net;
using YottySuba.Database;

namespace YottySuba.Components.Services;

public sealed class ApplicationContextService(IHttpContextAccessor contextAccessor, YottysubaContext databaseContext)
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
    private readonly YottysubaContext _databaseContext = databaseContext;


    public string ClientIpAddress { get; init; } = contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;


    public bool IsAddressBanned(IPAddress address)
    {
        var ipBytes = address.GetAddressBytes();
        return (from bans in _databaseContext.Bans
                where bans.Ip.Equals(ipBytes)
                select bans).Any();
    }

    public Ban? GetBan(IPAddress address)
    {
        var ipBytes = address.GetAddressBytes();
        return (from bans in _databaseContext.Bans
            where bans.Ip.Equals(ipBytes)
            select bans).FirstOrDefault();
    }
}
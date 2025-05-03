namespace YottySuba.Components.Services;

public static class TripcodeHelper
{
    public static string GenerateTripcode(string input)
    {
        if (!input.Contains('#')) return input; // no trip
        var parts = input.Split('#');
        var name = parts[0];
        var secret = parts.Length > 1 ? parts[1] : "";
        var hash = System.Security.Cryptography.SHA1.HashData(System.Text.Encoding.UTF8.GetBytes(secret));
        var trip = Convert.ToBase64String(hash)[..10];
        return $"{name} !{trip}";
    }
}
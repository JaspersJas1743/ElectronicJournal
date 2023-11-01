using ElectronicJournal.API.DBModels;

namespace ElectronicJournal.API.Utilities.Security.JWT
{
    public interface IJwtProvider
    {
        string Generate(User tokenOwner);
        Task<string> GenerateAsync(User tokenOwner);
    }
}

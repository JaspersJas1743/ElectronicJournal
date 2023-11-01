namespace ElectronicJournal.API.Utilities.Security.Hash
{
    public interface IHashProvider
    {
        string GenerateHash(string toHash);
        bool VerifyHash(string toHash, string hashedData);

        Task<string> GenerateHashAsync(string toHash);
        Task<bool> VerifyHashAsync(string toHash, string hashedData);
    }
}

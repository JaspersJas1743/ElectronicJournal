namespace ElectronicJournal.API.Utilities.Security.Hash
{
    public class HashProvider : IHashProvider
    {
        public string GenerateHash(string toHash)
            => BCrypt.Net.BCrypt.HashPassword(inputKey: toHash);

        public async Task<string> GenerateHashAsync(string toHash)
            => await Task.Run(function: () => GenerateHash(toHash: toHash));

        public bool VerifyHash(string toHash, string hashedData)
            => BCrypt.Net.BCrypt.Verify(text: toHash, hash: hashedData);

        public async Task<bool> VerifyHashAsync(string toHash, string hashedData)
            => await Task.Run(function: () => VerifyHash(toHash: toHash, hashedData: hashedData));
    }
}

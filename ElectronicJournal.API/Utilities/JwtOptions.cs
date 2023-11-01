using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ElectronicJournal.API.Utilities
{
    public class JwtOptions
    {
        private static bool _isInitialized = false;

        private string SecretKey { get; set; } = null!;
        public static JwtOptions Instance { get; private set; } = null!;
        public SymmetricSecurityKey SymmetricKey { get; private set; } = null!;
        public string Issuer { get; private set; } = null!;
        public string Audience { get; private set; } = null!;

        public static void Init(IConfiguration configuration)
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            IConfigurationSection jwtOptionsSection = configuration.GetSection(key: nameof(JwtOptions));

            Instance = new JwtOptions
            {
                Issuer = jwtOptionsSection.GetValue<string>(key: nameof(Issuer)),
                Audience = jwtOptionsSection.GetValue<string>(key: nameof(Audience)),
                SecretKey = jwtOptionsSection.GetValue<string>(key: nameof(SecretKey))
            };
            Instance.SymmetricKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: Instance.SecretKey));
        }
    }
}

using ElectronicJournalAPI.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournalAPI
{
    public class User
    {
        #region Constructor
        public User(int id, string surname, string name, string patronymic, string role, string gender, DateTime birthday, string phone, string email)
        {
            Id = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Role = role;
            Gender = gender;
            Birthday = birthday;
            Phone = phone;
            Email = email;
        }
        #endregion Constructor

        #region Properties
        public int Id { get; private set; }
        public string Surname { get; private set; }
        public string Name { get; private set; }
        public string Patronymic { get; private set; }
        public string Role { get; private set; }
        public string Gender { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Photo { get; private set; }
        #endregion Properties

        #region Methods
        internal static async Task<User> Create()
            => await ApiClient.GetAsync<User>(apiMethod: "User/GetInfo");

        public async Task DownloadProfilePhoto(CancellationToken cancellationToken = default)
        {
            try
            {
                TempFile file = new TempFile(fileName: $"id{Id}_profile_photo.jpg");
                if (file.Exists)
                {
                    Photo = file.FullName;
                    return;
                }

                byte[] response = await ApiClient.GetBytesAsync(apiMethod: "User/DownloadProfilePhoto");
                using (FileStream stream = new FileStream(path: file.FullName, mode: FileMode.Create))
                    await stream.WriteAsync(buffer: response, offset: 0, count: response.Length, cancellationToken: cancellationToken);
                Photo = file.FullName;
            }
            catch { return; }
        }
        #endregion Methods
    }
}

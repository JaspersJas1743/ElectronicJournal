using ElectronicJournalAPI.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournalAPI.ApiEntities
{
    public class User
    {
        #region Constructor
        public User(int id, string surname, string name, string patronymic, string role, string gender, DateTime birthday, string phone, string email)
            => (Id, Surname, Name, Patronymic, Role, Gender, Birthday, Phone, Email) = (id, surname, name, patronymic, role, gender, birthday, phone, email);
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

        #region Classes
        private class ChangePasswordRequest
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
        }

        private class ChangeEmailRequest
        {
            public string NewEmail { get; set; }
        }

        private class ChangePhoneRequest
        {
            public string NewPhone { get; set; }
        }

        public class ChangeResponse
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
        }

        public class MessageReceiversRequest
        {
            public string Filter { get; set; }
        }

        public class MessageReceiversResponse
        {
            public int Id { get; set; }
            public string DisplayedName { get; set; }
        }
        #endregion Classes

        #region Methods
        internal static async Task<User> Create()
            => await ApiClient.GetAsync<User>(apiMethod: "User/GetInfo");

        public async Task DownloadProfilePhoto(CancellationToken cancellationToken = default)
        {
            try
            {
                TempDirectory profilePhotoDirectory = new TempDirectory();
                profilePhotoDirectory.CreateIfNotExists();
                if (profilePhotoDirectory.TryFindFile(fileName: $"id{Id}_profile_photo", file: out TempFile photo))
                {
                    Photo = photo.FullName;
                    return;
                }

                byte[] response = await ApiClient.GetBytesAsync(apiMethod: "User/DownloadProfilePhoto");
                string fileExtension = ApiClient.ContentType.Split(separator: '/').Last();
                TempFile file = new TempFile(folder: profilePhotoDirectory, file: $"id{Id}_profile_photo.{fileExtension}");
                await file.WriteAsync(buffer: response, cancellationToken: cancellationToken);
                Photo = file.FullName;
            }
            catch { return; }
        }

        public async Task UploadProfilePhoto(string path, CancellationToken cancellationToken = default)
        {
            TempDirectory profilePhotoDirectory = new TempDirectory();
            profilePhotoDirectory.CreateIfNotExists();

            FileInfo fileToUpload = new FileInfo(fileName: path);
            TempFile file = new TempFile(folder: profilePhotoDirectory, file: $"id{Id}_profile_photo{fileToUpload.Extension}");
            fileToUpload.CopyTo(destFileName: file.FullName);

            await ApiClient.PostFileAsync(
                apiMethod: "User/UploadProfilePhoto",
                path: file.FullName
            );

            Photo = file.FullName;
        }

        public async Task<ChangeResponse> ChangePassword(string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            return await ApiClient.PutAsync<ChangeResponse, ChangePasswordRequest>(
                apiMethod: "User/ChangePassword",
                arg: new ChangePasswordRequest() { CurrentPassword = currentPassword, NewPassword = newPassword },
                cancellationToken: cancellationToken
            );
        }

        public async Task<ChangeResponse> ChangeEmail(string newEmail, CancellationToken cancellationToken = default)
        {
            ChangeResponse response = await ApiClient.PutAsync<ChangeResponse, ChangeEmailRequest>(
                apiMethod: "User/ChangeEmail",
                arg: new ChangeEmailRequest() { NewEmail = newEmail },
                cancellationToken: cancellationToken
            );

            if (response.IsSuccess)
                Email = newEmail;

            return response;
        }

        public async Task<ChangeResponse> ChangePhone(string newPhone, CancellationToken cancellationToken = default)
        {
            ChangeResponse response = await ApiClient.PutAsync<ChangeResponse, ChangePhoneRequest>(
                apiMethod: "User/ChangePhone",
                arg: new ChangePhoneRequest() { NewPhone = newPhone },
                cancellationToken: cancellationToken
            );

            if (response.IsSuccess)
                Phone = newPhone;

            return response;
        }

        public async Task<IEnumerable<MessageReceiversResponse>> GetReceivers(string filter, CancellationToken cancellationToken = default)
        {
            return await ApiClient.GetAsync<IEnumerable<MessageReceiversResponse>>(
                apiMethod: "Messages/GetMessageReceivers",
                argQuery: new Dictionary<string, string> { [nameof(MessageReceiversRequest.Filter)] = filter },
                cancellationToken: cancellationToken
            );
        }

        public async Task<IEnumerable<Message>> GetMessages(string dest, int offset, int count, int userId = 0, CancellationToken cancellationToken = default)
        {
            return await ApiClient.GetAsync<IEnumerable<Message>>(
                apiMethod: $"Messages/Get{dest}Messages",
                argQuery: new Dictionary<string, string>
                {
                    ["IsFiltered"] = (userId != 0).ToString(),
                    ["UserId"] = userId.ToString(),
                    ["Offset"] = offset.ToString(),
                    ["Count"] = count.ToString()
                },
                cancellationToken: cancellationToken
            );
        }

        public async Task<IEnumerable<Homework>> GetHomeworks(CancellationToken cancellationToken = default)
        {
            return await ApiClient.GetAsync<IEnumerable<Homework>>(
                apiMethod: "Homework/GetHomeworks",
                cancellationToken: cancellationToken
            );
        }

        public async Task<IEnumerable<Subject>> GetLessons(CancellationToken cancellationToken = default)
        {
            return await ApiClient.GetAsync<IEnumerable<Subject>>(
                apiMethod: "Lessons/GetLessons",
                cancellationToken: cancellationToken
            );
        }

        public void LogOut()
            => ApiClient.Token = null;
        #endregion Methods
    }
}

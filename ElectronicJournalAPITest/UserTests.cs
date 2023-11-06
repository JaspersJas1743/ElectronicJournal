namespace ElectronicJournalAPI.Test
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public async Task TestCase_User_1()
        {
            await TestAction(login: "Jaspers", password: "JaspersJas1743", expectedUser: new User(
                id: 1, surname: "Смирнов", name: "Алексей", patronymic: "Игоревич", role: "Студент", gender: "Мужской",
                birthday: new DateTime(year: 2005, month: 1, day: 13), phone: "+7(910)952-0836", email: "jsjs1743@mail.ru"
            ), photo: Path.Combine(path1: Path.GetTempPath(), path2: "ElectronicJournal", path3: $"id1_profile_photo.jpg"));
        }

        [TestMethod]
        public async Task TestCase_User_2()
        {
            await TestAction(login: "amudenn", password: "sad1324", photo: null, expectedUser: new User(
                id: 2, surname: "Демьянов", name: "Артём", patronymic: "Сергеевич", role: "Студент", gender: "Мужской",
                birthday: new DateTime(year: 2005, month: 6, day: 20), phone: "+7(920)858-1791", email: "amudenn@gmail.com"
            ));
        }

        [TestMethod]
        public async Task TestCase_User_3()
        {
            await TestAction(login: "dima", password: "55555555", photo: null, expectedUser: new User(
                id: 3, surname: "Виталев", name: "Дмитрий", patronymic: "Максимович", role: "Студент", gender: "Мужской",
                birthday: new DateTime(year: 2004, month: 11, day: 16), phone: "+7(926)140-7515", email: "plushechka4@mail.ru"
            ));
        }

        [TestMethod]
        public async Task TestCase_User_4()
        {
            await Assert.ThrowsExceptionAsync<AssertFailedException>(action: async () => await TestAction(login: "Jaspers", password: "JaspersJas1743",
                photo: null, expectedUser: new User(id: 3, surname: "Виталев", name: "Дмитрий", patronymic: "Максимович", role: "Студент",
                gender: "Мужской", birthday: new DateTime(year: 2004, month: 11, day: 16), phone: "+7(926)140-7515", email: "plushechka4@mail.ru"
            )));
        }

        [TestMethod]
        public async Task TestCase_User_5()
        {
            await Assert.ThrowsExceptionAsync<AssertFailedException>(action: async () => await TestAction(login: "Jaspers", password: "JaspersJas1743",
                photo: null, expectedUser: new User(id: 1, surname: "Смирнов", name: "Алексей", patronymic: "Игоревич", role: "Студент", gender: "Мужской",
                birthday: new DateTime(year: 2005, month: 1, day: 13), phone: "+7(910)952-0836", email: "jsjs1743@mail.ru"
            )));
        }

        private async Task TestAction(string login, string password, User expectedUser, string? photo)
        {
            AuthorizationModule authorization = AuthorizationModule.Create(login: login, password: password);
            User actual = await authorization.SignInAsync();
            Assert.AreEqual(expected: expectedUser.Id, actual: actual.Id);
            Assert.AreEqual(expected: expectedUser.Surname, actual: actual.Surname);
            Assert.AreEqual(expected: expectedUser.Name, actual: actual.Name);
            Assert.AreEqual(expected: expectedUser.Patronymic, actual: actual.Patronymic);
            Assert.AreEqual(expected: expectedUser.Role, actual: actual.Role);
            Assert.AreEqual(expected: expectedUser.Gender, actual: actual.Gender);
            Assert.AreEqual(expected: expectedUser.Birthday, actual: actual.Birthday);
            Assert.AreEqual(expected: expectedUser.Phone, actual: actual.Phone);
            Assert.AreEqual(expected: expectedUser.Email, actual: actual.Email);
            await actual.DownloadProfilePhoto();
            Assert.AreEqual(expected: photo, actual: actual.Photo);
        }
    }
}

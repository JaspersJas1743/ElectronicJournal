using ElectronicJournalAPI;

namespace ElectronicJournalAPITest
{
    [TestClass]
    public class AuthorizationModuleTests
    {
        [TestMethod]
        public async Task TestCase_AuthorizationModule_1()
            => await TestAction(login: "Jaspers", password: "JaspersJas1743");

        [TestMethod]
        public async Task TestCase_AuthorizationModule_2()
            => await Assert.ThrowsExceptionAsync<ApiException>(action: async () => await TestAction(login: "1234", password: "123456"));

        [TestMethod]
        public async Task TestCase_AuthorizationModule_3()
            => await Assert.ThrowsExceptionAsync<ApiException>(action: async () => await TestAction(login: "Jaspers", password: "123456"));

        [TestMethod]
        public async Task TestCase_AuthorizationModule_4()
            => await Assert.ThrowsExceptionAsync<ApiException>(action: async () => await TestAction(login: "123", password: "123456"));

        private async Task TestAction(string login, string password)
        {
            AuthorizationModule api = AuthorizationModule.Create(login: login, password: password);
            await api.SignInAsync();
        }
    }
}
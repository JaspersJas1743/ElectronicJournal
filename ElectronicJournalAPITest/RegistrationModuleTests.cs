namespace ElectronicJournalAPI.Test
{
    [TestClass]
    public class RegistrationModuleTests
    {
        [TestMethod]
        public async Task TestCase_Registration_RegistrationModule_1()
            => await Assert.ThrowsExceptionAsync<ApiException>(async () => await TestAction(code: String.Empty));

        [TestMethod]
        public async Task TestCase_Registration_RegistrationModule_2()
            => await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await TestAction(code: "123456"));

        [TestMethod]
        public async Task TestCase_Registration_RegistrationModule_3()
            => await TestAction(code: "unagxr");

        [TestMethod]
        public async Task TestCase_RegistrationOfAuthorizationData_RegistrationModule_1()
            => await Assert.ThrowsExceptionAsync<ApiException>(action: async () => await TestAction(login: "qwe", password: "qwerty"));

        [TestMethod]
        public async Task TestCase_RegistrationOfAuthorizationData_RegistrationModule_2()
            => await Assert.ThrowsExceptionAsync<ApiException>(action: async () => await TestAction(login: "qwerty", password: "qwe"));

        [TestMethod]
        public async Task TestCase_RegistrationOfAuthorizationData_RegistrationModule_3()
            => await Assert.ThrowsExceptionAsync<ApiException>(action: async () => await TestAction(login: "Jaspers", password: "zvkgowmvld"));

        [TestMethod]
        public async Task TestCase_RegistrationOfAuthorizationData_RegistrationModule_4()
            => await TestAction(login: "Iosif", password: "zvkgowmvld");

        private async Task TestAction(string login, string password)
        {
            RegistrationModule api = await RegistrationModule.Create(registrationCode: "unagxr");
            await api.SignUpAsync(login: login, password: password);
        }

        private async Task TestAction(string code)
            => await RegistrationModule.Create(registrationCode: code);
    }
}
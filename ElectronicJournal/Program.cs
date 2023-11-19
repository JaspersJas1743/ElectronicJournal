using ElectronicJournal.Models;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.Logger;
using ElectronicJournal.Utilities.Messages;
using ElectronicJournal.Utilities.Validator;
using ElectronicJournal.ViewModels;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prism.Events;
using System;

namespace ElectronicJournal
{
    public class Program
    {
        public static IHost AppHost { get; set; }

        [STAThread]
        public static void Main(string[] args)
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices(configureDelegate: services =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainWindowVM>();
                    services.AddSingleton<IEventAggregator, EventAggregator>();

                    services.AddTransient<MenuVM>();
                    services.AddTransient<AuthorizationVM>();
                    services.AddTransient<RegistrationVM>();
                    services.AddTransient<RegistrationOfAuthorizationDataVM>();
                    services.AddTransient<ProfileVM>();
                    services.AddTransient<MessagesVM>();
                    services.AddTransient<HomeworksVM>();
                    services.AddTransient<MarksVM>();
                    services.AddTransient<TimetableVM>();
                    services.AddTransient<AboutVM>();
                    services.AddTransient<MessageCreationVM>();
                    services.AddTransient<HomeworkViewerVM>();

                    services.AddScoped<IConfigProvider, ConfigurationProvider>();
                    services.AddScoped<IMessageProvider, MessageProvider>();
                    services.AddScoped<ILoggerProvider, LoggerProvider>();

                    services.AddScoped<IValidator<AuthorizationModel>, AuthorizationModelValidator>();
                    services.AddScoped<IValidator<RegistrationModel>, RegistrationModelValidator>();
                    services.AddScoped<IValidator<RegistrationOfAuthorizationDataModel>, RegistrationOfAuthorizationDataModelValidator>();
                    services.AddScoped<IValidator<ProfileModel.SecurityBlock>, ProfileModelSecurityValidator>();
                    services.AddScoped<IValidator<ProfileModel.EmailBlock>, ProfileModelEmailValidator>();
                    services.AddScoped<IValidator<ProfileModel.PhoneBlock>, ProfileModelPhoneValidator>();
                    services.AddScoped<IValidator<MessageCreationModel>, MessageCreationModelValidator>();
                }).Build();
            App app = AppHost.Services.GetService<App>();
            app.Run();
        }
    }
}

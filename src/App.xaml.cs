using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Login.WPF.Template.Models;
using Login.WPF.Template.ViewModels;
using Login.WPF.Template.Views;

namespace Login.WPF.Template
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // 注册服务和ViewModels
                    ConfigureServices(services);
                })
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // 注册服务
            services.AddSingleton<ILoginService, LoginService>();

            // 注册ViewModels
            services.AddSingleton<LoginViewModel>();

            // 注册视图
            services.AddSingleton<LoginView>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 启动 Host
            await AppHost.StartAsync();

            // 获取主窗口并显示
            var loginView = AppHost.Services.GetRequiredService<LoginView>();
            loginView.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);
        }
    }
}

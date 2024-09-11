using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using Login.WPF.Template.Models;
using Login.WPF.Template.Views;

namespace Login.WPF.Template.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ILoginService _loginService;

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
            // 加载保存的账号和密码
            var credentials = _loginService.LoadCredentials();
            Username = credentials.Username;
            Password = credentials.Password;
            RememberMe = credentials.RememberMe;
        }

        [ObservableProperty] string username;
        [ObservableProperty] string password;
        [ObservableProperty] bool rememberMe;

        [RelayCommand]
        async Task LoginClick()
        {
            var success = await _loginService.LoginAsync(Username, Password);
            if (success)
            {
                _loginService.SaveCredentials(Username, Password, RememberMe);
                ShowCustomMessageBox("Login Successful!", "You have successfully logged in.");

                // Navigate to the main application window
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.MainWindow.Close();
            }
            else
            {
                ShowCustomMessageBox("Login Failed", "Login failed, please check whether the user name and password are incorrect.");
            }

            await Task.CompletedTask;
        }

        [RelayCommand]
        private void CloseWindow()
        {
            Application.Current.MainWindow.Close();
        }

        [RelayCommand]
        private void MaximizeWindow()
        {
            Application.Current.MainWindow.WindowState =
                Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        [RelayCommand]
        private void MinimizeWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ShowCustomMessageBox(string title, string message)
        {
            var messageBox = new CustomMessageBox(title, message);
            messageBox.ShowDialog();
        }
    }
}

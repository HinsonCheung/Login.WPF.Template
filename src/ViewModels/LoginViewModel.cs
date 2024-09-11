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
            var responseType = await _loginService.LoginAsync(Username, Password);
            if (responseType.ResponseType == ResponseType.Successed)
            {
                ShowCustomMessageBox("Login Successful!", "You have successfully logged in.");
                _loginService.SaveCredentials(Username, Password, RememberMe);

                // Navigate to the main application window
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.MainWindow.Close();
            }
            else if(responseType.ResponseType == ResponseType.Failed)
            {
                ShowCustomMessageBox("Login Failed", "Login failed, please check whether the user name and password are incorrect.");
            }
            else if (responseType.ResponseType == ResponseType.Error)
            {
                ShowCustomMessageBox("登录请求失败", responseType.Message);
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

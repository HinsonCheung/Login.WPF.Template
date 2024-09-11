using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Login.WPF.Template.ViewModels
{
    public partial class CustomMessageBoxViewModel : ObservableObject
    {
        [ObservableProperty] string title;
        [ObservableProperty] string message;

        public CustomMessageBoxViewModel(string title, string message)
        {
            Title = title;
            Message = message;
        }

        [RelayCommand]
        private void Close()
        {
            Application.Current.Windows[Application.Current.Windows.Count - 1].Close();
        }
    }
}

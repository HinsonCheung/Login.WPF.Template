using System.Windows;
using Login.WPF.Template.ViewModels;

namespace Login.WPF.Template.Views
{
    /// <summary>
    /// CustomMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBoxViewModel ViewModel { get; }

        public CustomMessageBox(string title, string message)
        {
            ViewModel = new CustomMessageBoxViewModel(title, message);
            DataContext = this;

            InitializeComponent();
        }
    }
}

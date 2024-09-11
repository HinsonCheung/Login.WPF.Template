using System.Windows;
using System.Windows.Controls;

namespace Login.WPF.Template.Helpers
{
    public static class PasswordHelper
    {
        // BoundPassword 附加属性，用于绑定 PasswordBox 的 Password
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordHelper), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        // IsAttached 附加属性，表示是否附加了 PasswordChanged 事件
        public static readonly DependencyProperty IsAttached =
            DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, OnIsAttachedChanged));

        private static bool _isUpdating;

        // 设置 BoundPassword 的方法
        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPassword, value);
        }

        // 获取 BoundPassword 的方法
        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        // 设置 IsAttached 的方法
        public static void SetIsAttached(DependencyObject dp, bool value)
        {
            dp.SetValue(IsAttached, value);
        }

        // IsAttached 属性改变时触发，附加或移除事件
        private static void OnIsAttachedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if ((bool)e.OldValue)
                {
                    passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                }

                if ((bool)e.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                }
            }
        }

        // BoundPassword 属性改变时触发，用于更新 PasswordBox 的 Password
        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

                if (!_isUpdating)
                {
                    passwordBox.Password = (string)e.NewValue;
                }

                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        // PasswordBox PasswordChanged 事件处理程序
        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _isUpdating = true;
                SetBoundPassword(passwordBox, passwordBox.Password);
                _isUpdating = false;
            }
        }
    }
}

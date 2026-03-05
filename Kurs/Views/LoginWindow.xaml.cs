using System.Windows;
using System.Windows.Input;
using Kurs.Data;
using Microsoft.EntityFrameworkCore;

namespace Kurs.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            TxtUsername.Focus();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Login();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e) => Login();

        private void Login()
        {
            TxtError.Visibility = Visibility.Collapsed;
            var username = TxtUsername.Text.Trim();
            var password = TxtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите логин и пароль.");
                return;
            }

            try
            {
                using var db = new CarServiceContext();
                var user = db.Users
                    .Include(u => u.Employee)
                    .FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user == null)
                {
                    ShowError("Неверный логин или пароль.");
                    return;
                }

                App.CurrentUser = user;
                var main = new MainWindow();
                main.Show();
                Close();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка подключения к БД:\n{ex.Message}");
            }
        }

        private void ShowError(string msg)
        {
            TxtError.Text = msg;
            TxtError.Visibility = Visibility.Visible;
        }
    }
}

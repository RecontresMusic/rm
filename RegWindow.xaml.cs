using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace rm
{
    public partial class Window2 : Window
    {
        DataBaseConnect dbConnect = new DataBaseConnect();

        public Window2()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginField.Text;
            string password = PasswordBox.Password;  // Используем PasswordBox для более безопасной работы с паролями
            if (dbConnect.VerifyLogin(login, password))
            {
                MessageBox.Show("Login successful!");
            }
            else
            {
                MessageBox.Show("Login failed!");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginField.Text;
            string password = PasswordBox.Password;  // Используем PasswordBox для безопасности

            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
            {
                try
                {
                    dbConnect.RegisterUser(login, password);
                    MessageBox.Show("User registered successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error registering user: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Login and password cannot be empty.");
            }
        }

    }
}

using BlissEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace BlissEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string HOST = "blisseditorserver.postgres.database.azure.com";
        private static string PORT = "5432";
        private static string User = "BlissPG@blisseditorserver";
        private static string Password = "BlissEditor@";
        private static string DBName = "blisseditorDB";
        public MainWindow()
        {
            InitializeComponent();
            lblIncorrect.Visibility = Visibility.Hidden;
            txbPasswordShow.Visibility = Visibility.Hidden;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool dbExists;
            string usernameInput = txbUsername.Text;
            string passwordInput = txbPassword.Password;
            string connString = String.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer", HOST, User, DBName, PORT, Password);
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string cmdText = $"SELECT firstname FROM blisseditorusers WHERE username='{usernameInput}' AND password='{passwordInput}'";
                using (NpgsqlCommand cmd = new NpgsqlCommand(cmdText, conn))
                {
                    dbExists = cmd.ExecuteScalar() != null;
                }
            }

            if (dbExists)
            {
                /* Code Below from https://stackoverflow.com/questions/21706226/how-to-navigate-between-windows-in-wpf */
                var newWindow = new RTEWindow(); //create your new form.
                newWindow.Show(); //show the new form.
                this.Close(); //only if you want to close the current form.
            }
            else
            {
                lblIncorrect.Visibility = Visibility.Visible;
                txbPassword.Clear();
            }
        }

        private void txbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txbPasswordShow.Text = txbPassword.Password;
        }

        private void btnShow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txbPassword.Visibility = Visibility.Hidden;
            txbPasswordShow.Visibility= Visibility.Visible;
        }

        private void btnShow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txbPassword.Visibility = Visibility.Visible;
            txbPasswordShow.Visibility = Visibility.Hidden;
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new Window1();
            newWindow.Show();
            this.Close();
        }
    }
}
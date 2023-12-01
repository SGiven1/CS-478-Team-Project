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
using System.Windows.Shapes;
using Npgsql;

namespace BlissEditor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private static string HOST = "";
        private static string PORT = "";
        private static string User = "";
        private static string Password = "";
        private static string DBName = "";
        public Window1()
        {
            InitializeComponent();
            lblIncorrectSU.Visibility = Visibility.Hidden;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new MainWindow();
            newWindow.Show(); 
            this.Close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            bool dbExists;
            string usernameInput = txtUsernameSU.Text;
            string passwordInput = txtPasswordSU.Text;
            string firstnameInput = txtFirstNameSU.Text;
            string lastnameInput = txtLastNameSU.Text;
            string connString = String.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer", HOST, User, DBName, PORT, Password);
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string cmdText = $"SELECT firstname FROM blisseditorusers WHERE username='{usernameInput}'";
                using (NpgsqlCommand cmd = new NpgsqlCommand(cmdText, conn))
                {
                    dbExists = cmd.ExecuteScalar() != null;
                }
            }
            if (dbExists)
            {
                lblIncorrectSU.Content = "Username is taken";
                lblIncorrectSU.Visibility = Visibility.Visible;
            }
            else
            {
                if((usernameInput != "" || usernameInput != "Username") && (passwordInput != "" || passwordInput != "Password") && (firstnameInput != "" || firstnameInput != "First Name") && (lastnameInput !="" || lastnameInput != "Last Name"))
                {
                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();
                        using (var command = new NpgsqlCommand("INSERT INTO blisseditorusers (username, password, firstname, lastname) VALUES (@u1, @p1, @f1, @l1)", conn))
                        {
                            command.Parameters.AddWithValue("u1", usernameInput);
                            command.Parameters.AddWithValue("p1", passwordInput);
                            command.Parameters.AddWithValue("f1", firstnameInput);
                            command.Parameters.AddWithValue("l1", lastnameInput);
                            command.ExecuteNonQuery();
                        }
                    }
                    lblIncorrectSU.Visibility=Visibility.Visible;
                    lblIncorrectSU.Content = "Success";
                }
                else
                {
                    lblIncorrectSU.Visibility = Visibility.Visible;
                    lblIncorrectSU.Content = "Try again missing input";
                }
            }
        }
    }
}

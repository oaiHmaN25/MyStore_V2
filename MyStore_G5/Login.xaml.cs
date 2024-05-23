using Microsoft.Extensions.Configuration;
using MyStore_G5.Models;
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

namespace MyStore_G5
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private MyStoreContext con;
        public Login()
        {
            InitializeComponent();
            con = new MyStoreContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var account = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("account");
            var user = con.Staffs.FirstOrDefault(u => u.Name == username && u.Password == password);
            if(username == "" || password == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ tài khoản và mật khẩu!");
            }
            else if (username.Equals(account["username"]) && password.Equals(account["password"]))
            {
                Session.Username = username;
                var Admin = new Admin();
                Admin.Show();
                this.Close();
            }
            else
            {
                if (user != null)
                {
                    Session.Username = username;
                    Session.LoggedInStaffId = user.StaffId;
                    var Staff = new Staff();
                    Staff.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                }
            }
        }
    }
}

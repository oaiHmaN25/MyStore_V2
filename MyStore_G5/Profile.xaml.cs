using Microsoft.Extensions.Configuration;
using MyStore_G5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;
using System.Text.Json.Nodes;
using System.Security.Principal;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace MyStore_G5
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private MyStoreContext con;
        PasswordBox[] passwordBoxes = new PasswordBox[3];
        public Profile()
        {
            InitializeComponent();
            con = new MyStoreContext();
            if (!Session.Username.Equals("admin"))
            {
                var user = con.Staffs.FirstOrDefault(u => u.Name == Session.Username);
                UsernameLable.Content = user.Name;
                IdLable.Content = user.StaffId.ToString();
            }
            else
            {
                IdLable.Content = "1";
                UsernameLable.Content = Session.Username;
            }
            
            
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            PasswordStackPanel.Children.Clear();

            string[] labels = { "Old Password:", "New Password:", "Confirm Password:" };
            

            // Tạo và thêm ba PasswordBox mới
            for (int i = 0; i < 3; i++)
            {
                Label label = new Label();
                label.Content = labels[i];
                label.Margin = new Thickness(0, 5, 0, 0);
                label.HorizontalAlignment = HorizontalAlignment.Left;

                // Tạo ô nhập liệu
                PasswordBox passwordBox = new PasswordBox();
                passwordBox.Margin = new Thickness(0, 5, 0, 0);
                passwordBox.Width = 130;
                passwordBox.HorizontalAlignment = HorizontalAlignment.Left;
                passwordBoxes[i] = passwordBox;


                // Thêm Label và ô nhập liệu vào StackPanel
                PasswordStackPanel.Children.Add(label);
                PasswordStackPanel.Children.Add(passwordBox);

                if (i == 2)
                {
                    Button button = new Button();
                    button.Content = "Change";
                    button.Width = 50;
                    button.Margin = new Thickness(0, 5, 0, 0);
                    button.Click += new RoutedEventHandler(ChangePassClickEvent);
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    PasswordStackPanel.Children.Add(button);
                }
            }
        }

        private void ChangePassClickEvent(object sender, RoutedEventArgs e)
        {
            
            string oldPassword = passwordBoxes[0].Password;
            string newPassword = passwordBoxes[1].Password;
            string confPassWord = passwordBoxes[2].Password;

            if(oldPassword == "" || newPassword == "" || confPassWord == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ các trường thông tin!");
            }
            else if (!Session.Username.Equals("admin"))
            {
                MyStoreContext con = new MyStoreContext();
                var user = con.Staffs.FirstOrDefault(u => u.Name == Session.Username);
                if (oldPassword == user.Password)
                {
                    if (newPassword == oldPassword)
                    {
                        MessageBox.Show("Mật khẩu mới không được trùng với mật khẩu cũ!");
                    }
                    else if (newPassword != confPassWord)
                    {
                        MessageBox.Show("Mật khẩu mới không khớp nhau!");
                    }
                    else if (newPassword.Length<6)
                    {
                        MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!");
                    }
                    else
                    {
                        user.Password = newPassword;
                        con.SaveChanges();
                        MessageBox.Show("Thay đổi mật khẩu thành công!");
                        PasswordStackPanel.Children.Clear();
                    }

                }
                else
                {
                    MessageBox.Show("Mật khẩu không chính xác!");
                }
            }
            else
            {
                var account = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("account");
                if (oldPassword == account["password"])
                {
                    if (newPassword == oldPassword)
                    {
                        MessageBox.Show("Mật khẩu mới không được trùng với mật khẩu cũ!");
                    }
                    else if (newPassword != confPassWord)
                    {
                        MessageBox.Show("Mật khẩu mới không khớp nhau!");
                    }
                    else
                    {
                        string filePath = "appsettings.json";
                        string jsonString = File.ReadAllText(filePath);

                        JObject jsonObject = JObject.Parse(jsonString);
                        jsonObject["account"]["password"] = newPassword;

                        string updatedJsonString = jsonObject.ToString();
                        File.WriteAllText(filePath, updatedJsonString);
                        MessageBox.Show("Thay đổi mật khẩu thành công!");
                        PasswordStackPanel.Children.Clear();
                    }

                }
                else
                {
                    MessageBox.Show("Mật khẩu không chính xác!");
                }
            }
            
        }

    }
}

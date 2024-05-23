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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            lableWelcome.Visibility = Visibility.Hidden;
            Profile pro = new Profile();
            frameMain.Content = pro;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminReport adminReport = new AdminReport();
            frameMain.Content  = adminReport;
        }
        private void Logout(object sender, RoutedEventArgs e)
        {
            Session.Logout();
            this.Close();
        }

        private void Staff_Click(object sender, RoutedEventArgs e)
        {
            lableWelcome.Visibility = Visibility.Hidden;
            StaffManagement sm = new StaffManagement();
            frameMain.Content = sm;
        }

        private void frameMain_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}

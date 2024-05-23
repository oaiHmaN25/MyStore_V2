using MyStore_G5.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MyStore_G5
{
    /// <summary>
    /// Interaction logic for StaffOrder.xaml
    /// </summary>
    public partial class StaffOrder : Page
    {
        private ObservableCollection<OrderViewModel> orders;
        public StaffOrder()
        {
            InitializeComponent();
            int staffId = Session.LoggedInStaffId;  // Get the staff ID from your session or other source
            LoadOrders(staffId);
        }

        private void LoadOrders(int staffId)
        {
            // Retrieve orders for the specified staff ID from your data source
            using (var context = new MyStoreContext())
            {
                orders = new ObservableCollection<OrderViewModel>(
                    context.Orders.Where(o => o.StaffId == staffId)
                                  .Select(o => new OrderViewModel
                                  {
                                      OrderId = o.OrderId,
                                      OrderDate = o.OrderDate,
                                      StaffId = o.StaffId
                                  }).ToList());
            }

            // Set the ItemsSource of the ListView to your collection
            lvOrders.ItemsSource = orders;
        }



        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            // Get the current staff ID from the session
            int currentStaffId = Session.LoggedInStaffId;

            // Create and show the StaffAddOrder window, passing the current staff ID
            var addOrderWindow = new StaffAddOrder(currentStaffId);
            addOrderWindow.ShowDialog();
        }

        private void lvOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

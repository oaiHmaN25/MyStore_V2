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
    /// Interaction logic for StaffAddOrder.xaml
    /// </summary>
    public partial class StaffAddOrder : Window
    {
        private MyStoreContext _context;
        private int _currentStaffId;

        public StaffAddOrder(int currentStaffId)
        {
            InitializeComponent();
            _context = new MyStoreContext();
            _currentStaffId = currentStaffId;

            // Set the current date and staff ID
            datePickerOrderDate.SelectedDate = DateTime.Now;
            textBoxStaffId.Text = _currentStaffId.ToString();
            textBoxStaffId.IsEnabled = false; // Make the Staff ID field read-only
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input data
            DateTime orderDate = datePickerOrderDate.SelectedDate ?? DateTime.Now;

            // Create a new order
            var newOrder = new Order
            {
                OrderDate = orderDate,
                StaffId = _currentStaffId
            };

            // Add the new order to the database
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            // Close the AddOrderWindow
            this.DialogResult = true;
            this.Close();
        }
    }
}

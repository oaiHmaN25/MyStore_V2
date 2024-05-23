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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyStore_G5
{
    /// <summary>
    /// Interaction logic for StaffReport.xaml
    /// </summary>
    public partial class StaffReport : Page
    {
        private MyStoreContext _context;

        public StaffReport()
        {
            InitializeComponent();
            _context = new MyStoreContext();
            LoadOrders();
        }

        private void LoadOrders()
        {
            int staffId = Session.LoggedInStaffId;

            var orders = _context.Orders
                                 .Where(o => o.StaffId == staffId)
                                 .Select(o => new OrderViewModel
                                 {
                                     OrderId = o.OrderId,
                                     OrderDate = o.OrderDate,
                                     StaffId = o.StaffId,
                                     
                                 })
                                 .ToList();

            lvOrders.ItemsSource = orders;
            AssignButtonEvents();

        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the DataContext of the clicked button, which is the item bound to the row
            var button = sender as Button;
            var order = button.DataContext as OrderViewModel;

            if (order != null)
            {
                // Open the StaffOrderDetail window and pass the OrderId
                var orderDetailWindow = new StaffOrderDetail(order.OrderId);
                orderDetailWindow.ShowDialog();
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the DataContext of the clicked button, which is the item bound to the row
            var button = sender as Button;
            var order = button.DataContext as OrderViewModel;

            if (order != null)
            {
                // Confirm deletion
                var result = MessageBox.Show($"Are you sure you want to delete Order ID: {order.OrderId}?", "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Find the order entity in the context
                    var orderEntity = _context.Orders.SingleOrDefault(o => o.OrderId == order.OrderId);
                    if (orderEntity != null)
                    {
                        // Remove the order from the context and save changes
                        _context.Orders.Remove(orderEntity);
                        await _context.SaveChangesAsync();

                        // Refresh the list
                        LoadOrders();

                        MessageBox.Show($"Order ID: {order.OrderId} has been deleted.");
                    }
                }
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy ngày đã chọn từ DatePicker
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Now;

            // Lọc các đơn hàng theo ngày đã chọn
            var filteredOrders = GetOrdersByDate(selectedDate);

            // Cập nhật ListView với các đơn hàng đã lọc
            lvOrders.ItemsSource = filteredOrders;

            // Gán lại sự kiện cho các nút "Detail" và "Delete"
            AssignButtonEvents();
        }

        private void AssignButtonEvents()
        {
            // Lặp qua mỗi hàng trong ListView và gán lại sự kiện cho các nút "Detail" và "Delete"
            foreach (var item in lvOrders.Items)
            {
                if (lvOrders.ItemContainerGenerator.ContainerFromItem(item) is ListViewItem container)
                {
                    var detailButton = FindVisualChild<Button>(container, "DetailButton");
                    if (detailButton != null)
                    {
                        detailButton.Click -= DetailButton_Click;
                        detailButton.Click += DetailButton_Click;
                    }

                    var deleteButton = FindVisualChild<Button>(container, "DeleteButton");
                    if (deleteButton != null)
                    {
                        deleteButton.Click -= DeleteButton_Click;
                        deleteButton.Click += DeleteButton_Click;
                    }
                }
            }
        }

        private T FindVisualChild<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T dependencyObject && (child as FrameworkElement).Name == name)
                {
                    return dependencyObject;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child, name);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private List<Order> GetOrdersByDate(DateTime selectedDate)
        {
            using (var context = new MyStoreContext())
            {
                // Lọc các đơn hàng từ cơ sở dữ liệu theo ngày
                var orders = context.Orders
                    .Where(o => o.OrderDate.Date == selectedDate.Date)
                    .ToList();

                return orders;
            }
        }
    }
}


    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int StaffId { get; set; }
    }


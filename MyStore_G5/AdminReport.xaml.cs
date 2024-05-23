using Microsoft.EntityFrameworkCore;
using MyStore_G5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyStore_G5
{
    public partial class AdminReport : Page
    {
        private readonly MyStoreContext _context;

        public AdminReport()
        {
            InitializeComponent();
            _context = new MyStoreContext();
            LoadOrders();
            dpEndDate.SelectedDate = DateTime.Now.Date;
        }

        private void LoadOrders()
        {
            try
            {
                var orders = _context.Orders
                                     .Select(o => new
                                     {
                                         o.OrderId,
                                         o.OrderDate,
                                         o.StaffId,
                                         TotalPrice = o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice)
                                     })
                                     .AsEnumerable()
                                     .Select(o => new Order
                                     {
                                         OrderId = o.OrderId,
                                         OrderDate = o.OrderDate,
                                         StaffId = o.StaffId,
                                        // TotalPrice = o.TotalPrice
                                     })
                                     .ToList();

                lvOrders.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lvOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var orderId = (int)button.Tag;
                var orderDetail = new AdminOrderDetail(orderId);
                orderDetail.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening order detail: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var order = button.Tag as Order;

                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this order?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    var orderToRemove = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderId == order.OrderId);
                    if (orderToRemove != null)
                    {
                        _context.OrderDetails.RemoveRange(orderToRemove.OrderDetails);
                        _context.Orders.Remove(orderToRemove);
                        _context.SaveChanges();

                        LoadOrders();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }*/

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;

            if (startDate == null)
            {
                MessageBox.Show("Please select a start date.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Set default end date to system date if not selected
            if (endDate == null)
            {
                endDate = DateTime.Now.Date;
                dpEndDate.SelectedDate = endDate; // Set the selected date in the DatePicker
            }

            var filteredOrders = SearchOrdersByDateRange(startDate.Value, endDate.Value);
            lvOrders.ItemsSource = filteredOrders;
        }


        private List<Order> SearchOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            var orders = _context.Orders
                                 .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                                 .Select(o => new
                                 {
                                     o.OrderId,
                                     o.OrderDate,
                                     o.StaffId,
                                     TotalPrice = o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice)
                                 })
                                 .AsEnumerable()
                                 .Select(o => new Order
                                 {
                                     OrderId = o.OrderId,
                                     OrderDate = o.OrderDate,
                                     StaffId = o.StaffId,
                                     //TotalPrice = o.TotalPrice
                                 })
                                 .ToList();

            return orders;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MyStore_G5.Models;
using System;
using System.Linq;
using System.Windows;

namespace MyStore_G5
{
    public partial class AdminOrderDetail : Window
    {
        private readonly MyStoreContext _context;
        public int OrderId { get; }

        public AdminOrderDetail(int orderId)
        {
            InitializeComponent();
            _context = new MyStoreContext();
            OrderId = orderId;
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            try
            {
                var order = _context.Orders
                                    .Where(o => o.OrderId == OrderId)
                                    .Include(o => o.OrderDetails)
                                    .ThenInclude(od => od.Product)
                                    .Select(o => new
                                    {
                                        o.OrderId,
                                        o.OrderDate,
                                        o.StaffId,
                                        TotalPrice = o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice),
                                        OrderDetails = o.OrderDetails.Select(od => new
                                        {
                                            od.Product.ProductName,
                                            od.Quantity,
                                            od.UnitPrice,
                                            Total = od.Quantity * od.UnitPrice
                                        }).ToList()
                                    })
                                    .FirstOrDefault();

                if (order != null)
                {
                    DataContext = new
                    {
                        order.OrderId,
                        order.OrderDate,
                        order.StaffId,
                        order.TotalPrice
                    };
                    lvOrderDetails.ItemsSource = order.OrderDetails;
                }
                else
                {
                    MessageBox.Show("Order not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

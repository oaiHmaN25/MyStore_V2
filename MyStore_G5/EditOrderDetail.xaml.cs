using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for EditOrderDetail.xaml
    /// </summary>
   
        public partial class EditOrderDetail : Window
        {
            private OrderDetail editedOrderDetail;
        private MyStoreContext _context;
        public EditOrderDetail(OrderDetail orderDetail)
            {
            _context = new MyStoreContext();
            InitializeComponent();
                // Gán đối tượng OrderDetail được chuyển từ cửa sổ gọi vào editedOrderDetail
                editedOrderDetail = orderDetail;
                // Gán editedOrderDetail làm DataContext của cửa sổ
                DataContext = editedOrderDetail;
                 
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem liệu số lượng đã nhập có phải là số nguyên dương hay không
            if (!int.TryParse(quantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a positive integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Tiếp tục xử lý tiếp nếu số lượng hợp lệ

            // Retrieve the existing OrderDetail object from the database
            var existingOrderDetail = _context.OrderDetails.FirstOrDefault(od => od.OrderDetailId == editedOrderDetail.OrderDetailId);

            if (existingOrderDetail != null)
            {
                // Update the properties of the existing OrderDetail object
                existingOrderDetail.Quantity = quantity;

                // Update TotalPrice (assuming it's stored in the database)
                //existingOrderDetail.Total = existingOrderDetail.Quantity * existingOrderDetail.UnitPrice;

                // Save changes to the database using Entity Framework Core
                try
                {
                    _context.SaveChanges();
                    MessageBox.Show("Order detail updated successfully.");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                // Đóng cửa sổ mà không lưu thay đổi
                Close();
            }
        }
    }

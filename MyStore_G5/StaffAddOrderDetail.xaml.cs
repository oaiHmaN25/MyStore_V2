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
    /// Interaction logic for StaffAddOrderDetail.xaml
    /// </summary>
    public partial class StaffAddOrderDetail : Window
    {
        private readonly int _orderId;
        private MyStoreContext _context;

        public StaffAddOrderDetail(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            _context = new MyStoreContext();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _context.Categories.ToList();
            comboBoxCategory.ItemsSource = categories;
            comboBoxCategory.DisplayMemberPath = "CategoryName";
            comboBoxCategory.SelectedValuePath = "CategoryId";
        }

        private void ComboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxCategory.SelectedValue is int selectedCategoryId)
            {
                var products = _context.Products
                                       .Where(p => p.CategoryId == selectedCategoryId)
                                       .ToList();
                comboBoxProduct.ItemsSource = products;
                comboBoxProduct.DisplayMemberPath = "ProductName";
                comboBoxProduct.SelectedValuePath = "ProductId";
            }
        }

        private void ComboBoxProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxProduct.SelectedItem is Product selectedProduct)
            {
                textBoxUnitPrice.Text = selectedProduct.UnitPrice.ToString();
            }
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input data
            if (comboBoxProduct.SelectedValue is int productId &&
                int.TryParse(textBoxQuantity.Text, out int quantity) &&
                int.TryParse(textBoxUnitPrice.Text, out int unitPrice))
            {
                // Kiểm tra nếu quantity không phải là số nguyên dương
                if (quantity <= 0 || !IsInteger(textBoxQuantity.Text))
                {
                    MessageBox.Show("Quantity must be a positive integer.");
                    return;
                }

                // Create a new order detail using MyStore_G5.Models.OrderDetail
                var newOrderDetail = new MyStore_G5.Models.OrderDetail
                {
                    OrderId = _orderId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };

                // Add the new order detail to the database
                _context.OrderDetails.Add(newOrderDetail);
                await _context.SaveChangesAsync();

                // Close the AddOrderDetail window
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter valid data.");
            }
        }

        private bool IsInteger(string value)
        {
            // Sử dụng TryParse để kiểm tra xem chuỗi có phải là số nguyên hay không
            // Nếu TryParse trả về true, có nghĩa là chuỗi là số nguyên
            return int.TryParse(value, out int result);
        }
    }
}

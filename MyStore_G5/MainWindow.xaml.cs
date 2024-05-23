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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyStoreContext())
            {
                var productsWithCategories = context.Products
                    .Select(p => new { p.ProductName, p.Category.CategoryName }) // Lựa chọn tên sản phẩm và tên danh mục
                    .ToList();

                string message = "Products:\n";
                foreach (var product in productsWithCategories)
                {
                    message += $"Product: {product.ProductName}, Category: {product.CategoryName}\n";
                }

                MessageBox.Show(message, "Products", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

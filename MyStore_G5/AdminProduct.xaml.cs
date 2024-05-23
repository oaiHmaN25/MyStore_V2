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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyStore_G5
{
    /// <summary>
    /// Interaction logic for AdminProduct.xaml
    /// </summary>
    public partial class AdminProduct : Page
    {
        private List<Product> productList; // Define the list of products
        MyStoreContext context = new MyStoreContext();
        public AdminProduct()
        {
            InitializeComponent();
            loadData();
            loadCategory();
        }

        private void loadData()
        {
            MyStoreContext _context = new MyStoreContext();
            productList = _context.Products.ToList(); // Load products from your data source
            lvProducts.ItemsSource = productList; // Bind to ListView
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private bool blankInput()
        {
            if (txtProductId.Text.Length < 1 || txtProductName.Text.Length < 1 || txtCategoryId.Text.Length < 1
                 || txtUnitPrice.Text.Length < 1) return false;
            return true;
        }

        private Product GetProduct()
        {
            Product product = new Product();
            product.ProductName = txtProductName.Text;
            if (int.TryParse(txtCategoryId.Text, out int categoryId))
            {
                product.CategoryId = categoryId;
            }
            else
            {
                product.CategoryId = 0; // Default value
            }
            if (int.TryParse(txtUnitPrice.Text, out int unitPrice))
            {
                product.UnitPrice = unitPrice;
            }
            else
            {
                product.UnitPrice = 0; // Default value
            }
            return product;
        }

        private void loadCategory()
        {
            txtCategoryId.ItemsSource = context.Categories.ToList();
            txtCategoryId.SelectedIndex = 0;
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (blankInput() == false)
            {
                MessageBox.Show("Information not empty");
                return;
            }
            Product product = GetProduct();
            MyStoreContext _context = new MyStoreContext();
            _context.Products.Add(product);
            _context.SaveChanges();
            loadData();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (blankInput() == false)
            {
                MessageBox.Show("Information not empty");
                return;
            }
            Product product = GetProduct();
            product.ProductId = int.Parse(txtProductId.Text);
            MyStoreContext _context = new MyStoreContext();
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            loadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to delete?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Product product = GetProduct();
                product.ProductId = int.Parse(txtProductId.Text);
                MyStoreContext _context = new MyStoreContext();
                _context.Products.Remove(product);
                _context.SaveChanges();
                loadData();
            }
            else
            {
                return;
            }
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            List<Product> filteredProducts = GetFilteredProducts(searchTerm);

            // Bind filteredProducts to your ListView
            lvProducts.ItemsSource = filteredProducts;
        }

        private List<Product> GetFilteredProducts(string searchTerm)
        {
            // Filter products based on ID or name
            return productList.Where(p => p.ProductId.ToString().Contains(searchTerm) || p.ProductName.ToLower().Contains(searchTerm)).ToList();
        }

        private void txtSearchByUnit_TextChanged(object sender, TextChangedEventArgs e)
        {
            string unitPriceSearchTerm = txtSearchByUnit.Text.Trim();
            List<Product> filteredProducts = GetFilteredProductsByUnitPrice(unitPriceSearchTerm);

            lvProducts.ItemsSource = filteredProducts;
        }

        private List<Product> GetFilteredProductsByUnitPrice(string unitPriceSearchTerm)
        {
            // Filter products based on unit price
            return productList.Where(p => p.UnitPrice.ToString().Contains(unitPriceSearchTerm)).ToList();
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
